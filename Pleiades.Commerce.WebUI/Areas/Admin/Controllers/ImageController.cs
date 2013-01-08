using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Data;
using Pleiades.Web;
using Pleiades.Web.FineUploader;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Resources;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class ImageController : Controller
    {
        IImageBundleRepository ImageBundleRepository { get; set; }
        IFileResourceRepository FileResourceRepository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public ImageController(
                IImageBundleRepository imageBundleRepository, 
                IFileResourceRepository fileResourceRepository, 
                IUnitOfWork unitOfWork)
        {
            this.ImageBundleRepository = imageBundleRepository;
            this.FileResourceRepository = fileResourceRepository;
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult UploadTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var stream = file.InputStream;
            var bitmap = new Bitmap(file.InputStream);
            var imageBundle = this.ImageBundleRepository.Add(bitmap);

            return new JsonNetResult(imageBundle);
        }

        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {
            try
            {
                var bitmap = new Bitmap(upload.InputStream);
                var imageBundle = this.ImageBundleRepository.Add(bitmap);
                this.UnitOfWork.SaveChanges();
                return new FineUploaderResult(true, new { extraInformation = imageBundle });
            }
            catch (Exception ex)
            {
                // TODO: dump this to logs and handle AJAX errors appropriately 
                return new FineUploaderResult(false, error: ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Download(Guid externalResourceId, string size)
        {
            var imageBundle = this.ImageBundleRepository.Retrieve(externalResourceId);
            FileResource fileResource;
            if (size == "thumbnail")
                fileResource = imageBundle.Thumbnail;
            else if (size == "large")
                fileResource = imageBundle.Large;
            else if (size == "small")
                fileResource = imageBundle.Small;
            else 
                fileResource = imageBundle.Original;

            var path = this.FileResourceRepository.PhysicalFilePath(fileResource.ExternalId);
            
            // TODO: add more MIME-types?
            return base.File(path, "image/jpeg");
        }
    }
}