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
            var bundleID = product.Images.Any() ? 
                product.Images.OrderBy(x => x.Order).First().ImageBundle.ExternalId.ToString() : 
                (string)null;

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
                SizeGroupId = product.SizeGroup == null ? (int?)null : product.SizeGroup.ID,
                SizeGroupName = product.SizeGroup == null ? "" : product.SizeGroup.Name,
                UnitPrice = product.UnitPrice,
                UnitCost = product.UnitCost,
                ImageBundleExternalId = bundleID ?? Guid.Empty.ToString(),
                AssignImagesToColors = product.AssignImagesToColors,
            };
        }

        public static JsonProductColor ToJson(this ProductColor color)
        {
            return new JsonProductColor
            {
                Id = color.Id,
                Name = color.Color.Name,
                SEO = color.Color.SEO,
                SkuCode = color.Color.SkuCode,
                ImageBundleExternalId = color.Color.ImageBundle != null ? color.Color.ImageBundle.ExternalId.ToString() : null,
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