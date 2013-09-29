using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using Pleiades.Application.Data;
using Pleiades.Application.Logging;
using Pleiades.Web;
using Pleiades.Web.FineUploader;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Resources;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageBundleRepository _imageBundleRepository;
        private readonly IFileResourceRepository _fileResourceRepository;
        private readonly IBlankImageRepository _blankImageRepository;
    
        IUnitOfWork UnitOfWork { get; set; }

        public ImageController(
                IImageBundleRepository imageBundleRepository, 
                IFileResourceRepository fileResourceRepository, 
                IBlankImageRepository blankImageRepository,
                IUnitOfWork unitOfWork)
        {
            this._imageBundleRepository = imageBundleRepository;
            this._fileResourceRepository = fileResourceRepository;
            this._blankImageRepository = blankImageRepository;
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
            var imageBundle = this._imageBundleRepository.AddBitmap(bitmap);

            return new JsonNetResult(imageBundle);
        }

        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {
            try
            {
                var bitmap = new Bitmap(upload.InputStream);
                var imageBundle = this._imageBundleRepository.AddBitmap(bitmap);
                this.UnitOfWork.SaveChanges();
                return new FineUploaderResult(true, new { ImageBundle = imageBundle });
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
            LoggerSingleton.Get().Debug("Image - Download: " + externalResourceId + " " + size);
            var imageBundle = this._imageBundleRepository.Retrieve(externalResourceId);
            if (imageBundle == null)
            {
                var path = _blankImageRepository.BlankImageBySize(size.ToImageSize());

                // TODO: introduce Physical File type, which contains file extension
                return base.File(path, "image/gif");
            }
            else
            {
                var fileResource = imageBundle.FileByImageSize(size.ToImageSize());
                var path = this._fileResourceRepository.PhysicalFilePath(fileResource.ExternalId);

                // TODO: map file types on upload
                return base.File(path, "image/jpg");
            }
        }
    }
}