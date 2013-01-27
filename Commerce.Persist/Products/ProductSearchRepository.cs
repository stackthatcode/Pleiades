using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Products;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Products
{
    public class ProductSearchRepository : IProductSearchRepository
    {
        PleiadesContext Context { get; set; }

        public ProductSearchRepository(PleiadesContext context)
        {
            this.Context = context;
        }

        public IQueryable<Product> Data()
        {
            return this.Context.Products
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Include(x => x.Images.Select(img => img.ImageBundle));
        }

        public List<JsonProductSynopsis> FindProducts(int? categoryId, int? brandId, string searchText)
        {
            var query = this.Data();
            if (categoryId != null)
            {
                query = query.Where(x => x.Category.Id == categoryId || x.Category.ParentId == categoryId);
            }
            if (brandId != null)
            {
                query = query.Where(x => x.Brand.Id == brandId);
            }
            if (searchText != null)
            {
                searchText = "%" + searchText.Replace(" ", "%") + "%";
                query = query.Where(x =>
                    SqlFunctions.PatIndex(searchText, x.Name) > 0 ||
                    SqlFunctions.PatIndex(searchText, x.SkuCode) > 0 ||
                    SqlFunctions.PatIndex(searchText, x.Synopsis) > 0 ||
                    SqlFunctions.PatIndex(searchText, x.Description) > 0);
            }

            return query.ToList().Select(x => x.ToSynopsis()).ToList();
        }

        public JsonProductSynopsis Retrieve(int productId)
        {
            return this.Data().FirstOrDefault(x => x.Id == productId).ToSynopsis();
        }
    }
}