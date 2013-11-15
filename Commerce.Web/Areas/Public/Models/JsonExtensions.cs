using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Lists.Entities;
using Commerce.Application.Products;
using Commerce.Application.Products.Entities;
using Pleiades.Web.Json;

namespace Commerce.Web.Areas.Public.Models
{
    public static class JsonExtensions
    {
        public static JsonNetResult BuildProductDetailJson(JsonProductInfo productInfo,
                JsonBrand brandInfo, List<JsonProductColor> colors, List<ProductSize> sizes, List<JsonProductImage> images, List<ProductSku> inventory)
        {
            var result = new
            {
                Info = new
                {
                    productInfo.Id,
                    productInfo.Name,
                    productInfo.CategoryId,
                    productInfo.CategoryName,
                    productInfo.Description,
                    productInfo.ImageBundleExternalId,
                    productInfo.AssignImagesToColors,
                    productInfo.SkuCode,
                    productInfo.Synopsis,
                    productInfo.UnitPrice,
                },
                Inventory = inventory.Select(
                    item => new
                    {
                        Id = item.Id,
                        SizeId = item.Size != null ? item.Size.Id : (int?)null,
                        ColorId = item.Color != null ? item.Color.Id : (int?)null,
                        SkuCode = item.OriginalSkuCode,
                        Quantity = item.InStock,
                    }),
                Images = images.Select(
                    image => new
                    {
                        Id = image.Id,
                        ColorId = image.ProductColorId,
                        ImageBundleExternalId = image.ImageBundleExternalId,
                        Order = image.Order,
                    }),
                Colors = colors.Select(
                    color => new
                    {
                        Id = color.Id,
                        ImageBundleExternalId = color.ImageBundleExternalId,
                        Name = color.Name,
                        color.Order,
                    }
                ),
                Brand = brandInfo == null ? null : new
                {
                    brandInfo.Id,
                    brandInfo.ImageBundleExternalId,
                    brandInfo.Name,
                    brandInfo.Description,
                    brandInfo.SEO,
                },
                Sizes = sizes.Select(
                    size => new
                    {
                        Id = size.Id,
                        Abbr = size.Abbr,
                        Name = size.Name,
                        SkuCode = size.SkuCode,
                        Order = size.Order,
                    })
            };
            return new JsonNetResult(result);
        }
    }
}