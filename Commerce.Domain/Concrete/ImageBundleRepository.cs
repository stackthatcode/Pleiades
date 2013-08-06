using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Data.Entity;
using Commerce.Persist.Database;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Concrete
{
    public class ImageBundleRepository : IImageBundleRepository
    {
        private static ConcurrentDictionary<int, ImageBundle> _cacheById = new ConcurrentDictionary<int, ImageBundle>();
        private static ConcurrentDictionary<Guid, ImageBundle> _cacheByGuid = new ConcurrentDictionary<Guid, ImageBundle>();

        public PushMarketContext Context { get; set; }
        public IFileResourceRepository FileResourceRepository { get; set; }
        public IImageProcessor ImageProcessor { get; set; }

        public ImageBundleRepository(PushMarketContext context,
                IFileResourceRepository fileResourceRepository, IImageProcessor imageProcessor)
        {
            this.Context = context;
            this.FileResourceRepository = fileResourceRepository;
            this.ImageProcessor = imageProcessor;
        }

        protected IQueryable<ImageBundle> Data()
        {
            return this.Context.ImageBundles
                .Where(x => x.Deleted == false)
                .Include(x => x.Large)
                .Include(x => x.Original)
                .Include(x => x.Thumbnail)
                .Include(x => x.Small);
        }

        public ImageBundle Add(Bitmap original)
        {
            var thumbnail = this.ImageProcessor.CreateThumbnail(original);
            var large = this.ImageProcessor.CreateLarge(original);
            var small = this.ImageProcessor.CreateSmall(original);

            var bundle = new ImageBundle()
            {
                ExternalId = Guid.NewGuid(),
                Large = this.FileResourceRepository.AddNew(large),
                Original = this.FileResourceRepository.AddNew(original),
                Thumbnail = this.FileResourceRepository.AddNew(thumbnail),
                Small = this.FileResourceRepository.AddNew(small),
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.ImageBundles.Add(bundle);
            return bundle;
        }

        public ImageBundle Add(Color color, int width, int height)
        {
            var original = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(original))
            using (SolidBrush brush = new SolidBrush(color))
            {
                gfx.FillRectangle(brush, 0, 0, width, height);
            };

            var thumbnail = this.ImageProcessor.CreateThumbnail(original);
            var small = this.ImageProcessor.CreateSmall(original);
            var large = this.ImageProcessor.CreateLarge(original);

            var bundle = new ImageBundle()
            {
                ExternalId = Guid.NewGuid(),
                Large = this.FileResourceRepository.AddNew(large),
                Original = this.FileResourceRepository.AddNew(original),
                Thumbnail = this.FileResourceRepository.AddNew(thumbnail),
                Small = this.FileResourceRepository.AddNew(small),
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.ImageBundles.Add(bundle);
            return bundle;
        }

        public ImageBundle Copy(int Id)
        {
            var original = this.Retrieve(Id);
            var copy = new ImageBundle()
            {
                ExternalId = Guid.NewGuid(),
                Large = this.FileResourceRepository.Copy(original.Large),
                Small = this.FileResourceRepository.Copy(original.Small),
                Original = this.FileResourceRepository.Copy(original.Original),
                Thumbnail = this.FileResourceRepository.Copy(original.Thumbnail),
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.ImageBundles.Add(copy);
            return copy;
        }

        public ImageBundle Retrieve(int Id)
        {
            if (_cacheById.ContainsKey(Id))
            {
                return _cacheById[Id];
            }
            var output = this.Data().FirstOrDefault(x => x.Id == Id);
            _cacheById.TryAdd(output.Id, output);
            _cacheByGuid.TryAdd(output.ExternalId, output);
            return output;
        }

        public ImageBundle Retrieve(Guid externalId)
        {
            if (_cacheByGuid.ContainsKey(externalId))
            {
                return _cacheByGuid[externalId];
            }
            var output = this.Data().FirstOrDefault(x => x.ExternalId == externalId);
            if (output == null)
            {
                return new ImageBundle { ExternalId = Guid.Empty };
            }

            _cacheById.TryAdd(output.Id, output);
            _cacheByGuid.TryAdd(output.ExternalId, output);
            return output;
        }

        public void Delete(int Id)
        {
            var imageBundle = this.Retrieve(Id);
            imageBundle.Original.Deleted = true;
            imageBundle.Thumbnail.Deleted = true;
            imageBundle.Large.Deleted = true;
            imageBundle.Small.Deleted = true;
            imageBundle.Deleted = true;

            imageBundle.Original.DateUpdated = DateTime.Now;
            imageBundle.Thumbnail.DateUpdated = DateTime.Now;
            imageBundle.Large.DateUpdated = DateTime.Now;
            imageBundle.Small.DateUpdated = DateTime.Now;
            imageBundle.DateUpdated = DateTime.Now;
        }
    }
}