using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Concrete
{
    public class JsonColorRepository : IJsonColorRepository
    {
        PleiadesContext Context { get; set; }
        IImageBundleRepository ImageBundleRepository { get; set; }

        public JsonColorRepository(PleiadesContext context, IImageBundleRepository imageBundleRepository)
        {
            this.Context = context;
            this.ImageBundleRepository = imageBundleRepository;
        }

        protected IQueryable<Color> Data()
        {
            return this.Context.Colors
                .Where(x => x.Deleted == false)
                .OrderBy(x => x.Name.ToUpper())
                .Include(x => x.ImageBundle);
        }

        public List<JsonColor> RetrieveAll()
        {
            var data = this.Data().ToList();
            return data.Select(x => x.ToJson()).ToList();
        }

        public JsonColor Retrieve(int id)
        {
            var color = this.Data().First(x => x.Id == id);
            return color.ToJson();
        }

        public void Update(JsonColor colorDiff)
        {
            var imageBundle = this.ImageBundleRepository.Retrieve(Guid.Parse(colorDiff.ImageBundleExternalId));
            var color = this.Data().FirstOrDefault(x => x.Id == colorDiff.Id);

            color.Name = colorDiff.Name;
            color.SkuCode = colorDiff.SkuCode;
            color.SEO = colorDiff.SEO;
            color.ImageBundle = imageBundle;
            color.DateUpdated = DateTime.Now;

            // EF 5.0 paranoia
            this.Context.MarkModified(color);
        }

        public Func<JsonColor> Insert(JsonColor brandDiff)
        {
            var imageBundle = this.ImageBundleRepository.Retrieve(Guid.Parse(brandDiff.ImageBundleExternalId));

            var color = new Color
            {
                Name = brandDiff.Name,
                SkuCode = brandDiff.SkuCode,
                SEO = brandDiff.SEO,
                ImageBundle = imageBundle,
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.Colors.Add(color);
            return () => color.ToJson();
        }

        public void DeleteSoft(JsonColor ColorDiff)
        {
            var Color = this.Data().FirstOrDefault(x => x.Id == ColorDiff.Id);
            Color.Deleted = true;
            Color.DateUpdated = DateTime.Now;
            this.Context.MarkModified(Color);
        }
    }
}