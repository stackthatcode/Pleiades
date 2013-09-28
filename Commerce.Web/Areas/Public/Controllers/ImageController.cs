using System;
using System.Web.Mvc;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Resources;
using Pleiades.Application.Logging;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageBundleRepository _imageBundleRepository;
        private readonly IFileResourceRepository _fileResourceRepository;
        private readonly IBlankImageRepository _blankImageRepository;

        public ImageController(
                IImageBundleRepository imageBundleRepository,
                IFileResourceRepository fileResourceRepository,
                IBlankImageRepository blankImageRepository)
        {
            _imageBundleRepository = imageBundleRepository;
            _fileResourceRepository = fileResourceRepository;
            _blankImageRepository = blankImageRepository;
        }

        // GET api/product/5
        [HttpGet]
        [ActionName("action-with-id")]
        public FilePathResult Get(Guid identifier, string size)
        {
            var results = _imageBundleRepository.Retrieve(identifier);
            var file = results.FileByImageSize(size.ToImageSize());
            var path = _fileResourceRepository.PhysicalFilePath(file.ExternalId);
            return base.File(path, "image/jpeg");
        }

        [HttpGet]
        public ActionResult Download(Guid externalResourceId, string size)
        {
            LoggerSingleton.Get().Debug("Image - Download: " + externalResourceId + " " + size);

            // ALSO: why are we doing this stuff in a controller...?
            var imageBundle = this._imageBundleRepository.Retrieve(externalResourceId);
            if (imageBundle == null)
            {
                // TODO: introduce Physical File type, which contains file extension
                var path = _blankImageRepository.BlankImageBySize(size.ToImageSize());
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
