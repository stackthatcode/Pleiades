using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Commerce.Application.File;
using Commerce.Application.File.Entities;
using Pleiades.App.Data;
using Pleiades.App.Logging;
using Pleiades.App.Utility;
using Pleiades.Web.FineUploader;
using Pleiades.Web.Json;

namespace Commerce.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageBundleRepository _imageBundleRepository;
        private readonly IFileResourceRepository _fileResourceRepository;
        private readonly IBlankImageRepository _blankImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly bool _azureHosted;

        public ImageController(
                IImageBundleRepository imageBundleRepository, 
                IFileResourceRepository fileResourceRepository, 
                IBlankImageRepository blankImageRepository,
                IUnitOfWork unitOfWork)
        {
            this._imageBundleRepository = imageBundleRepository;
            this._fileResourceRepository = fileResourceRepository;
            this._blankImageRepository = blankImageRepository;
            this._unitOfWork = unitOfWork;
            _azureHosted = ConfigurationManager.AppSettings["AzureHosted"].ToBoolTryParse();
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
            var imageBundle = this._imageBundleRepository.AddBitmap(bitmap, true, true, false);

            return new JsonNetResult(imageBundle);
        }

        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {
            try
            {
                var bitmap = new Bitmap(upload.InputStream);
                var imageBundle = this._imageBundleRepository.AddBitmap(bitmap, true, true, false);
                this._unitOfWork.SaveChanges();
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
                if (_azureHosted)
                {
                    var bytes = this._fileResourceRepository.RetrieveBytes(fileResource.ExternalId);
                    return new FileStreamResult(new MemoryStream(bytes), "image/jpg");
                }
                else
                {
                    var path = this._fileResourceRepository.PhysicalFilePath(fileResource.ExternalId);
                    return base.File(path, "image/jpg");
                }
            }
        }
    }
}