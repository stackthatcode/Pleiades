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
    public class JsonBrandRepository : IJsonBrandRepository
    {
        PleiadesContext Context { get; set; }
        IImageBundleRepository ImageBundleRepository { get; set; }

        public JsonBrandRepository(PleiadesContext context, IImageBundleRepository imageBundleRepository)
        {
            this.Context = context;
            this.ImageBundleRepository = imageBundleRepository;
        }

        protected IQueryable<Brand> Data()
        {
            return 
                this.Context.Brands
                    .Where(x => x.Deleted == false)
                    .Include(x => x.ImageBundle);
        }

        public List<JsonBrand> RetrieveAll()
        {
            var data = this.Data().ToList();
            return data.Select(x => x.ToJson()).ToList();
        }

        public JsonBrand Retrieve(int id)
        {
            var brand = this.Data().First(x => x.Id == id);
            return brand.ToJson();
        }

        public void Update(JsonBrand brandDiff)
        {
            var imageBundle = this.ImageBundleRepository.Retrieve(Guid.Parse(brandDiff.ImageBundleExternalId));
            var brand = this.Data().FirstOrDefault(x => x.Id == brandDiff.Id);
            brand.Name = brandDiff.Name;
            brand.Description = brandDiff.Description;
            brand.SkuCode = brandDiff.SkuCode;
            brand.SEO = brandDiff.SEO;
            brand.ImageBundle = imageBundle;
            brand.DateUpdated = DateTime.Now;
        }

        public Func<JsonBrand> Insert(JsonBrand brandDiff)
        {
            var imageBundle = 
                this.ImageBundleRepository.Retrieve(Guid.Parse(brandDiff.ImageBundleExternalId));

            var brand = new Brand
            {
                Name = brandDiff.Name,
                Description = brandDiff.Description,
                SkuCode = brandDiff.SkuCode,
                SEO = brandDiff.SEO,
                ImageBundle = imageBundle,
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.Brands.Add(brand);
            return () => brand.ToJson();
        }

        public void DeleteSoft(JsonBrand brandDiff)
        {
            var brand = this.Data().FirstOrDefault(x => x.Id == brandDiff.Id);
            brand.Deleted = true;
            brand.DateUpdated = DateTime.Now;
            brand.ImageBundle.Deleted = true;
            brand.ImageBundle.DateUpdated = DateTime.Now;
        }
    }
}