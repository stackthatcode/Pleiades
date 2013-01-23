using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Domain.Model.Products
{
    public static class ProductExtensions
    {
        public static JsonProductSynopsis ToSynopsis(this Product product)
        {
            var bundleID = product.Images.Any() ? 
                product.Images.OrderBy(x => x.Order).First().ImageBundle.ExternalId.ToString() : 
                (string)null;

            return new JsonProductSynopsis
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
                ImageBundleExternalId = bundleID,
            };
        }
    }
}