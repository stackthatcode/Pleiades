using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Persist.Database;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;

namespace Commerce.Persist.Concrete
{
    public class JsonBrandRepository : IJsonBrandRepository
    {
        PushMarketContext Context { get; set; }
        IImageBundleRepository ImageBundleRepository { get; set; }

        public JsonBrandRepository(PushMarketContext context, IImageBundleRepository imageBundleRepository)
        {
            this.Context = context;
            this.ImageBundleRepository = imageBundleRepository;
        }


        internal class ProductsPerBrand
        {
            public Brand Brand { get; set; }
            public int Count { get; set; }
        }

        internal IQueryable<ProductsPerBrand> ProductCountQuery()
        {
            return
                this.Context.Products
                    .Include(x => x.Brand)
                    .Where(x => x.Brand != null && x.IsDeleted == false)
                    .GroupBy(x => x.Brand)
                    .Select(x => new ProductsPerBrand { Brand = x.Key, Count = x.Count() });
        }

        protected IQueryable<Brand> Data()
        {            
            var result =
                this.Context.Brands
                    .Where(x => x.Deleted == false)
                    .Include(x => x.ImageBundle);

            return result;
        }

        public List<JsonBrand> RetrieveAll()
        {
            var data = this.Data().ToList();
            var result = data
                .Select(x => x.ToJson())
                .OrderBy(x => x.Name.ToUpper())
                .ToList();

            var productCount = ProductCountQuery().ToList();

            result.ForEach(x => x.ProductCount = 
                    productCount.Any(pcount => pcount.Brand.Id == x.Id) 
                        ? productCount.First(pcount => pcount.Brand.Id == x.Id).Count : 0);
            return result;
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
            this.Context.MarkModified(brand);
            
            var products =
                this.Context.Products
                    .Where(x => x.Brand != null)
                    .Where(x => x.Brand.Id == brand.Id)
                    .ToList();

            foreach (var product in products)
            {
                product.Brand = null;

                this.Context.MarkModified(product);
            }
        }
    }
}