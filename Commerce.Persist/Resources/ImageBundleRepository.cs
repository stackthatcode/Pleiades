using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Resources;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Resources
{
    public class ImageBundleRepository : IImageBundleRepository
    {
        public PleiadesContext Context { get; set; }
        public IFileResourceRepository FileResourceRepository { get; set; }
        public IImageProcessor ImageProcessor { get; set; }

        public ImageBundleRepository(PleiadesContext context,
                IFileResourceRepository fileResourceRepository, IImageProcessor imageProcessor)
        {
            this.Context = context;
            this.FileResourceRepository = fileResourceRepository;
            this.ImageProcessor = imageProcessor;
        }

        public ImageBundle Add(Bitmap original)
        {
            var thumbnail = this.ImageProcessor.CreateThumbnail(original);
            var large = this.ImageProcessor.CreateLarge(original);

            var bundle = new ImageBundle()
            {
                ExternalId = Guid.NewGuid(),
                Large = this.FileResourceRepository.AddNew(large),
                Original = this.FileResourceRepository.AddNew(original),
                Thumbnail = this.FileResourceRepository.AddNew(thumbnail),
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.ImageBundles.Add(bundle);
            return bundle;
        }

        public ImageBundle Retrieve(int Id)
        {
            return Context.ImageBundles.FirstOrDefault(x => x.Id == Id);
        }

        public ImageBundle Retrieve(Guid externalId)
        {
            return Context.ImageBundles.FirstOrDefault(x => x.ExternalId == externalId);
        }

        public void Delete(int Id)
        {
            var imageBundle = this.Retrieve(Id);
            imageBundle.Original.Deleted = true;
            imageBundle.Thumbnail.Deleted = true;
            imageBundle.Large.Deleted = true;
            imageBundle.Deleted = true;

            imageBundle.Original.DateUpdated = DateTime.Now;
            imageBundle.Thumbnail.DateUpdated = DateTime.Now;
            imageBundle.Large.DateUpdated = DateTime.Now;
            imageBundle.DateUpdated = DateTime.Now;
        }
    }
}