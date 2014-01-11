using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using Commerce.Application.File;
using Commerce.Application.File.Entities;
using Pleiades.App.Logging;
using Pleiades.App.Utility;

namespace ArtOfGroundFighting.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageBundleRepository _imageBundleRepository;
        private readonly IFileResourceRepository _fileResourceRepository;
        private readonly IBlankImageRepository _blankImageRepository;
        private readonly bool _azureHosted;

        public ImageController(
                IImageBundleRepository imageBundleRepository,
                IFileResourceRepository fileResourceRepository,
                IBlankImageRepository blankImageRepository)
        {
            _imageBundleRepository = imageBundleRepository;
            _fileResourceRepository = fileResourceRepository;
            _blankImageRepository = blankImageRepository;
            _azureHosted = ConfigurationManager.AppSettings["AzureHosted"].ToBoolTryParse();
        }

        // GET api/product/5
        [HttpGet]
        [ActionName("action-with-id")]
        public ActionResult Get(Guid id, string size)
        {
            LoggerSingleton.Get().Debug("Image - Download: " + id + " " + size);

            var imageBundle = this._imageBundleRepository.Retrieve(id);
            if (imageBundle == null)
            {
                var path = _blankImageRepository.BlankImageBySize(size.ToImageSize());
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
