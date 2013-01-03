using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Lists
{
    public class BrandRepository : IBrandRepository
    {
        DbContext Context { get; set; }

        public BrandRepository(DbContext context)
        {
            this.Context = context;
        }

        protected IQueryable<Brand> Data()
        {
            return this.Context.Set<Brand>().Where(x => x.Deleted == false);
        }

        public List<Brand> RetrieveAll()
        {
            return this.Data().ToList();
        }

        public void Update(Brand brandDiff)
        {
            var brand = this.Data().FirstOrDefault(x => x.Id == brandDiff.Id);
            brand.Name = brandDiff.Name;
            brand.Description = brandDiff.Description;
            brand.SkuCode = brandDiff.SkuCode;
            brand.SEO = brandDiff.SEO;
            brand.DateUpdated = DateTime.Now;
        }

        public Func<Brand> Insert(Brand brandDiff)
        {
            var brand = new Brand
            {
                Name = brandDiff.Name,
                Description = brandDiff.Description,
                SkuCode = brandDiff.SkuCode,
                SEO = brandDiff.SEO,
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.Set<Brand>().Add(brand);
            return () => brand;
        }

        public void DeleteSoft(Brand brandDiff)
        {
            var brand = this.Data().FirstOrDefault(x => x.Id == brandDiff.Id);
            brand.Deleted = true;
            brand.DateUpdated = DateTime.Now;
        }
    }
}