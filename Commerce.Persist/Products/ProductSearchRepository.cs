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
                .Include(x => x.SizeGroup)
                .Include(x => x.Images)
                .Include(x => x.Images.Select(img => img.ImageBundle));
        }

        public List<JsonProductInfo> FindProducts(int? categoryId, int? brandId, string searchText)
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

            return query.ToList().Select(x => x.ToJson()).ToList();
        }

        public JsonProductInfo RetrieveInfo(int productId)
        {
            return this.Data().FirstOrDefault(x => x.Id == productId).ToJson();
        }

        public List<JsonProductColor> RetreieveColors(int productId)
        {
            return this.Context.ProductColors
                .Include(x => x.Color)
                .Include(x => x.Color.ImageBundle)
                .Where(x => x.Product.Id == productId && x.IsDeleted == false)
                .ToList()
                .Select(x => x.ToJson())
                .ToList();
        }

        // Two sequences
        // 1.) Choose an existing Color
        // 2.) Create a new Color, add it to the global Color List, then run this method
        
        public JsonProductColor AddProductColor(int productId, int colorId)
        {
            var color = this.Context.Colors.First(x => x.Deleted == false && x.Id == colorId);
            var product = this.Context.Products.First(x => x.Id == productId);

            var productColor = 
                this.Context.ProductColors.Add(new ProductColor
                {
                    Color = color,
                    Product = product,
                });

            return productColor.ToJson();
        }

        public void DeleteProductColor(int productId, int productColorId)
        {
            // TODO: add logic for addressing Product Images attached to a Color

            var productColor = this.Context.ProductColors.First(x => x.Product.Id == productId && x.Id == productColorId);
            productColor.IsDeleted = true;
        }
    }
}