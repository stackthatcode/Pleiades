using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Products;
using Commerce.Persist.Model.Resources;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Concrete
{
    /// <summary>
    /// More of a ProductService, so-to-speak.  Manages the entire Bounded Context of Products
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        PleiadesContext Context { get; set; }
        IImageBundleRepository ImageBundleRepository { get; set; }
        IInventoryRepository InventoryRepository { get; set; }

        public ProductRepository(PleiadesContext context, 
                IImageBundleRepository imageBundleRepository, IInventoryRepository inventoryRepository)
        {
            this.Context = context;
            this.ImageBundleRepository = imageBundleRepository;
            this.InventoryRepository = inventoryRepository;
        }


        // Query bases
        private IQueryable<Product> ProductWithImagesBrandsCategories()
        {
            return this.Context.Products
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Images)
                .Include(x => x.Images.Select(img => img.ImageBundle))
                .Include(x => x.Images.Select(img => img.ProductColor));
        }

        private Product ProductWithColorsAndSizes(int id)
        {
            return
                this.Context.Products
                    .Include(x => x.ThumbnailImageBundle)
                    .Include(x => x.Sizes)
                    .Include(x => x.Colors)
                    .Include(x => x.Colors.Select(color => color.ProductImageBundle))
                    .First(x => x.Id == id && x.IsDeleted == false);
        }

        private Product ProductWithColorsAndImages(int id)
        {
            var product = this.Context.Products
                   .Include(x => x.ThumbnailImageBundle)
                   .Include(x => x.Colors)
                   .Include(x => x.Images)
                   .Include(x => x.Images.Select(img => img.ProductColor))
                   .Include(x => x.Images.Select(img => img.ImageBundle))
                   .FirstOrDefault(x => x.Id == id);
            return product;
        }


        // Info + Search
        public List<JsonProductInfo> FindProducts(int? categoryId, int? brandId, string searchText)
        {
            var query = this.ProductWithImagesBrandsCategories();
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
            return this.ProductWithImagesBrandsCategories().FirstOrDefault(x => x.Id == productId).ToJson();
        }

        public void Delete(int productId)
        {
            var product = this.Context.Products.First(x => x.Id == productId);
            product.IsDeleted = true;

            // TODO: delete the Inventory...?  I think the answer is yes, I don't know
        }


        // Colors - coupled to Inventory Context
        public List<JsonProductColor> RetreiveColors(int productId)
        {
            var product = this.Context.Products
                .Include(x => x.Colors)
                .Include(x => x.Colors.Select(color => color.ColorImageBundle))
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
                //.Include(x => x.Sizes)
                .First(x => x.Id == productId);

            var imageBundle = this.ImageBundleRepository.Copy(color.ImageBundle.Id);
            var order = product.Colors.Count() == 0 ? 1 : product.Colors.Max(x => x.Order) + 1;

            var productColor = new ProductColor
            {
                ColorImageBundle = imageBundle,
                Name = color.Name,
                SEO = color.SEO,
                SkuCode = color.SkuCode,
                Order = order,
            };

            // *** Wipe Inventory if this is the first Color
            if (product.Colors.Count() == 0)
            {
                this.InventoryRepository.Wipe(product.Id);
            }

            product.Colors.Add(productColor);
            return () => productColor.ToJson();
        }

        public void UpdateProductColorSort(int productId, string sortedIds)
        {
            var product = this.ProductWithColorsAndImages(productId);
            var colors = product.Colors;
            var idList = sortedIds.Split(',').Select(x => Int32.Parse(x)).ToList();

            for (var index = 0; index < idList.Count(); index++)
            {
                var color = colors.FirstOrDefault(x => x.Id == idList[index]);
                color.Order = index;
            }

            product.SetThumbnailImages();
            this.Context.SaveChanges();
        }

        public void DeleteProductColor(int productId, int productColorId)
        {
            var product = this.ProductWithColorsAndImages(productId);
            var productColor = this.Context.ProductColors.First(x => x.Id == productColorId);
            
            foreach (var image in product.Images)
            {
                if (image.ProductColor != null && image.ProductColor.Id == productColorId)
                {
                    image.ProductColor = null;
                }
            }

            this.InventoryRepository.DeleteByColor(productId, productColorId);
            productColor.IsDeleted = true;
            product.Colors.Remove(productColor);
            product.SetThumbnailImages();
        }


        // Sizes - coupled to Inventory Context
        private Product RetrieveProductForSizes(int productId)
        {
            var product = this.Context.Products
                .Include(x => x.Colors)
                .Include(x => x.Sizes)
                .First(x => x.Id == productId && x.IsDeleted == false);
            return product;
        }

        public List<ProductSize> RetrieveSizes(int productId)
        {            
            return this.RetrieveProductForSizes(productId)
                .Sizes
                .OrderBy(x => x.Order)
                .ToList();
        }

        public Func<ProductSize> AddProductSize(int productId, int sizeId)
        {
            var product = this.RetrieveProductForSizes(productId);
            var size = this.Context.Sizes.First(x => x.ID == sizeId);

            var order = product.Sizes.Count() == 0 ? 1 : product.Sizes.Max(x => x.Order) + 1;
            var productSize = new ProductSize
                {
                    Abbr = size.Name,
                    Name = size.Description,
                    SkuCode = size.SkuCode,
                    Order = order,
                };

            product.Sizes.Add(productSize);

            // Is this the first Size?
            if (product.Sizes.Count() == 1)
            {
                this.InventoryRepository.Wipe(product.Id);
            }
            
            return () => productSize;
        }

        public void UpdateSizeOrder(int productId, string sortedIds)
        {
            var sizes = this.RetrieveProductForSizes(productId).Sizes;
            var idList = sortedIds.Split(',').Select(x => Int32.Parse(x)).ToList();

            for (var index = 0; index < idList.Count(); index++)
            {
                var size = sizes.FirstOrDefault(x => x.Id == idList[index]);
                size.Order = index;
            }

            this.Context.SaveChanges();
        }

        public Func<ProductSize> CreateProductSize(int productId, ProductSize productSize)
        {
            var product = this.RetrieveProductForSizes(productId);
            if (product.Sizes.Count() == 0)
            {
                this.InventoryRepository.Wipe(product.Id);
            } 
            product.Sizes.Add(productSize);            
            return () => productSize;
        }

        public void DeleteProductSize(int productId, int sizeId)
        {
            var product = this.RetrieveProductForSizes(productId);
            this.InventoryRepository.DeleteBySize(productId, sizeId);
            var size = product.Sizes.First(x => x.Id == sizeId);
            size.IsDeleted = true;
            product.Sizes.Remove(size);
        }


        // Images
        public List<JsonProductImage> RetrieveImages(int productId)
        {
            return this
                .ProductWithColorsAndImages(productId)
                .Images
                .OrderBy(x => x.Order)
                .Select(x => x.ToJson())
                .ToList();
        }

        public Func<JsonProductImage> AddProductImage(int productId, JsonProductImage image)
        {
            var externalId = Guid.Parse(image.ImageBundleExternalId);
            var imageBundle = this.Context.ImageBundles.First(x => x.ExternalId == externalId);

            var product = this.ProductWithColorsAndImages(productId);            
            var color = product.Colors.FirstOrDefault(x => x.Id == image.ProductColorId);
            var order = product.Images.Select(x => x.Order).Count() + 1;
            
            var productImage = new ProductImage()
            {
                ImageBundle = imageBundle,
                Order = order,
                ProductColor = color
            };
            product.Images.Add(productImage);
            
            if (product.ThumbnailImageBundle == null)
            {
                product.ThumbnailImageBundle = imageBundle;
            }

            product.SetThumbnailImages();
            return () => productImage.ToJson();
        }

        public void UpdateProductImageSort(int productId, string sortedIds)
        {
            var product = this.ProductWithColorsAndImages(productId);
            var idList = sortedIds.Split(',').Select(x => Int32.Parse(x)).ToList();

            for (var index = 0; index < idList.Count(); index++)
            {
                var image = product.Images.FirstOrDefault(x => x.Id == idList[index]);
                image.Order = index;
            }

            product.SetThumbnailImages();
            this.Context.SaveChanges();
        }

        public void DeleteProductImage(int productId, int imageId)
        {
            var product = this.ProductWithColorsAndImages(productId);
            var image = product.Images.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                image.ImageBundle.Deleted = true;
                product.Images.Remove(image);
            }

            product.SetThumbnailImages();
        }

        public void ChangeImageColor(int productId, int productImageId, int newColor)
        {
            var product = this.ProductWithColorsAndImages(productId);
            var image = product.Images.First(x => x.Id == productImageId);
            var color = product.Colors.FirstOrDefault(x => x.Id == newColor);
            var order = product.Images.Select(x => x.Order).Max() + 1;
            image.ProductColor = color;
            image.Order = order;
            product.SetThumbnailImages();
        }

        public void AssignImagesToColor(int productId)
        {
            var product = this.ProductWithColorsAndImages(productId);
            product.AssignImagesToColors = true;
            product.SetThumbnailImages();
        }

        public void UnassignImagesFromColor(int productId)
        {
            var product = this.ProductWithColorsAndImages(productId);
            product.AssignImagesToColors = false;
            product.Images.ForEach(x => x.ProductColor = null);
            product.SetThumbnailImages();
        }
    }
}
