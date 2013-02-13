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
                .OrderBy(x => x.Order)
                .Select(x => x.ToJson())
                .ToList();
        }

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
            var productColor = this.Context.ProductColors.First(x => x.Product.Id == productId && x.Id == productColorId);
            productColor.IsDeleted = true;

            var images = this.Context.Products
                .Include(x => x.Images)
                .Include(x => x.Images.Select(img => img.ProductColor))
                .First(x => x.Id == productId)
                .Images;

            foreach (var image in images)
            {
                if (image.ProductColor != null && image.ProductColor.Id == productColorId)
                {
                    image.ProductColor = null;
                }
            }
        }

        public void UpdateProductColorSort(int productId, string sortedIds)
        {
            var colors = this.Context.ProductColors
                .Where(x => x.IsDeleted == false && x.Product.Id == productId)
                .ToList();
            var idList = sortedIds.Split(',').Select(x => Int32.Parse(x)).ToList();

            for (var index = 0; index < idList.Count(); index++)
            {
                var color = colors.FirstOrDefault(x => x.Id == idList[index]);
                color.Order = index;
            }

            this.Context.SaveChanges();
        }

        public void UpdateProductImageSort(int productId, string sortedIds)
        {
            var images = this.Context.Products
                .Include(x => x.Images)
                .FirstOrDefault(x => x.Id == productId)
                .Images
                .ToList();
            var idList = sortedIds.Split(',').Select(x => Int32.Parse(x)).ToList();

            for (var index = 0; index < idList.Count(); index++)
            {
                var image = images.FirstOrDefault(x => x.Id == idList[index]);
                image.Order = index;
            }

            this.Context.SaveChanges();
        }

        public List<JsonProductImage> RetrieveImages(int productId)
        {
            return this.Context.Products
                .Include(x => x.Images)
                .Include(x => x.Images.Select(img => img.ImageBundle))
                .Include(x => x.Images.Select(img => img.ProductColor))
                .FirstOrDefault(x => x.Id == productId)
                .Images
                .OrderBy(x => x.Order)
                .Select(x => x.ToJson())
                .ToList();
        }

        public Func<JsonProductImage> AddProductImage(int productId, JsonProductImage image)
        {
            var externalId = Guid.Parse(image.ImageBundleExternalId);
            var imageBundle = this.Context.ImageBundles.First(x => x.ExternalId == externalId);
            var product = this.Context.Products.Include(x => x.Images).First(x => x.Id == productId);
            var order = product.Images.Select(x => x.Order).Max() + 1;
            
            var productImage = new ProductImage()
            {
                ImageBundle = imageBundle,
                Order = order,
                ProductColor = null
            };
            product.Images.Add(productImage);
            return () => productImage.ToJson();
        }

        public void DeleteProductImage(int productId, int imageId)
        {
            var product = this.Context.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == productId);
            var image = product.Images.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
                product.Images.Remove(image);
        }

        public void ChangeImageColor(int productId, int productImageId, int newColor)
        {
            var product = this.Context.Products
                    .Include(x => x.Images)
                    .Include(x => x.Images.Select(img => img.ProductColor))
                    .First(x => x.Id == productId);
            var image = product.Images.First(x => x.Id == productImageId);
            var color = this.Context.ProductColors.First(x => x.Product.Id == productId && x.Id == newColor);
            var order = product.Images.Select(x => x.Order).Max() + 1;
            image.ProductColor = color;
        }

        public void AssignImagesToColor(int productId)
        {
            var product = this.Context.Products
                .Include(x => x.Images)
                .First(x => x.Id == productId);

            // Idempotence
            if (product.AssignImagesToColors == true)
                return;

            var colors = this.Context.ProductColors.Where(x => x.Product.Id == productId);
            if (colors.Count() == 0)
                return;
        }

        public void UnassignImagesFromColor(int productId)
        {
            var product = this.Context.Products
               .Include(x => x.Images)
               .First(x => x.Id == productId);

            // Idempotence
            if (product.AssignImagesToColors == false)
                return;

            // Break the old association
            foreach (var image in product.Images)
            {
                image.ProductColor = null;
            }
        }
    }
}
