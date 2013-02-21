using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Model.Products
{
    public class ProductSku
    {
        public int Id { get; set; }
        
        public string SkuCode { get; set; }
        public string Name { get; set; }

        public Product Product { get; set; }
        public ProductColor Color { get; set; }
        public ProductSize Size { get; set; }
        public string OriginalSkuCode { get; set; }

        public int TotalInventory { get; set; }
        public int ReservedInventory { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime LastModified { get; set; }

        public static ProductSku Factory(Product product, ProductColor color, ProductSize size)
        {
            var skuCode = product.SkuCode;
            var name = product.Name;            
            if (color != null)
            {
                skuCode += "-" + color.SkuCode;
                name += " - " + color.Name; 
            }
            if (size != null)
            {
                skuCode += "-" + size.SkuCode;
                name += " - " + size.Name;
            }

            var productSku = new ProductSku
            {
                OriginalSkuCode = skuCode,
                SkuCode = skuCode,
                Name = name,
                Product = product,
                Color = color,
                Size = size,
                ReservedInventory = 0,
                TotalInventory = 0,
                IsDeleted = false,
                DateInserted = DateTime.Now,
                LastModified = DateTime.Now,
            };

            return productSku;
        }
    }
}