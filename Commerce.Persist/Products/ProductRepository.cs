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
    public class ProductRepository : IProductRepository
    {
        PleiadesContext Context { get; set; }
        IImageBundleRepository ImageBundleRepository { get; set; }

        public ProductRepository(PleiadesContext context, IImageBundleRepository imageBundleRepository)
        {
            this.Context = context;
            this.ImageBundleRepository = imageBundleRepository;
        }

        public IQueryable<Product> Data()
        {
            return this.Context.Products
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Include(x => x.Images.Select(img => img.ImageBundle))
                .Include(x => x.Images.Select(img => img.ProductColor));
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

        public void Delete(int productId)
        {
            var product = this.Context.Products.First(x => x.Id == productId);
            product.IsDeleted = true;
        }

        public List<JsonProductColor> RetreiveColors(int productId)
        {
            var product = this.Context.Products
                .Include(x => x.Colors)
                .Include(x => x.Colors.Select(color => color.ImageBundle))
                .First(x => x.Id == productId && x.IsDeleted == false);

            return product.Colors
                .OrderBy(x => x.Order)
                .Select(x => x.ToJson())
                .ToList();
        }

        public Func<JsonProductColor> AddProductColor(int productId, int colorId)
        {
            var color = this.Context.Colors
                .Include(x => x.ImageBundle)
                .First(x => x.Deleted == false && x.Id == colorId);

            var product = this.Context.Products
                .Include(x => x.Colors)
                .First(x => x.Id == productId);

            var imageBundle = this.ImageBundleRepository.Copy(color.ImageBundle.Id);

            var productColor = new ProductColor
            {
                ImageBundle = imageBundle,
                Name = color.Name,
                SEO = color.SEO,
                SkuCode = color.SkuCode,
                Order = product.Colors.Count() + 1,
            };

            product.Colors.Add(productColor);
            return () => productColor.ToJson();
        }

        public void DeleteProductColor(int productId, int productColorId)
        {
            var product = this.Context.Products
               .Include(x => x.Images)
               .Include(x => x.Colors)
               .Include(x => x.Colors.Select(color => color.ImageBundle))
               .First(x => x.Id == productId && x.IsDeleted == false);

            var productColor = this.Context.ProductColors.First(x => x.Id == productColorId);
            
            foreach (var image in product.Images)
            {
                if (image.ProductColor != null && image.ProductColor.Id == productColorId)
                {
                    image.ProductColor = null;
                }
            }
            product.Colors.Remove(productColor);
        }

        public void UpdateProductColorSort(int productId, string sortedIds)
        {
            var product = this.Context.Products
                  .Include(x => x.Colors)
                  .First(x => x.Id == productId);

            var colors = product.Colors;
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

            var product = this.Context.Products
                .Include(x => x.Images)
                .First(x => x.Id == productId);

            var color = this.Context.ProductColors.FirstOrDefault(x => x.Id == image.ProductColorId);
            var order = product.Images.Select(x => x.Order).Count() + 1;
            
            var productImage = new ProductImage()
            {
                ImageBundle = imageBundle,
                Order = order,
                ProductColor = color
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
                    .Include(x => x.Colors)
                    .Include(x => x.Images)
                    .Include(x => x.Images.Select(img => img.ProductColor))
                    .First(x => x.Id == productId);

            var image = product.Images.First(x => x.Id == productImageId);
            var color = product.Colors.First(x => x.Id == newColor);
            var order = product.Images.Select(x => x.Order).Max() + 1;
            image.ProductColor = color;
            image.Order = order;
        }

        public void AssignImagesToColor(int productId)
        {
            var product = this.Context.Products.First(x => x.Id == productId);
            product.AssignImagesToColors = true;

        }

        public void UnassignImagesFromColor(int productId)
        {
            var product = this.Context.Products.First(x => x.Id == productId);
            product.AssignImagesToColors = false;
        }
    }
}
