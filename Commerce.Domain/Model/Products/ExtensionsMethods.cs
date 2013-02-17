using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Domain.Model.Products
{
    public static class ExtensionsMethods
    {
        public static JsonProductInfo ToJson(this Product product)
        {
            var externalId = product.ThumbnailImage == null ? 
                Guid.Empty : product.ThumbnailImage.ImageBundle.ExternalId;

            return new JsonProductInfo
            {
                Id = product.Id,
                Name = product.Name,
                Synopsis = product.Synopsis,
                Description = product.Description,
                SEO = product.SEO,
                SkuCode = product.SkuCode,
                BrandId = product.Brand == null ? (int?)null : product.Brand.Id,
                BrandName = product.Brand == null ? "" : product.Brand.Name,
                CategoryId = product.Category == null ? (int?)null : product.Category.Id,
                CategoryName = product.Category == null ? "" : product.Category.Name,
                UnitPrice = product.UnitPrice,
                UnitCost = product.UnitCost,
                ImageBundleExternalId = externalId.ToString(),
                AssignImagesToColors = product.AssignImagesToColors,
            };
        }

        public static JsonProductColor ToJson(this ProductColor color)
        {
            return new JsonProductColor
            {
                Id = color.Id,
                Name = color.Name,
                SEO = color.SEO,
                SkuCode = color.SkuCode,
                ImageBundleExternalId = color.ImageBundle != null ? color.ImageBundle.ExternalId.ToString() : null,
                Order = color.Order,
            };
        }

        public static JsonProductImage ToJson(this ProductImage image)
        {
            return new JsonProductImage
            {
                Id = image.Id,
                ImageBundleExternalId = image.ImageBundle.ExternalId.ToString(),
                Order = image.Order,
                ProductColorId = image.ProductColor != null ? image.ProductColor.Id : 0,
                Description = image.Description,
            };
        }
    }
}