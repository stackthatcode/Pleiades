using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Model.Products
{
    public class ProductSku
    {
        public int Id { get; set; }        
        public string SkuCode { get; set; }
        public string Name { get; set; }

        public string OriginalSkuCode { get; set; }
        public Product Product { get; set; }
        public ProductColor Color { get; set; }
        public ProductSize Size { get; set; }

        public ImageBundle ImageBundle
        {
            get
            {
                if (this.Product.AssignImagesToColors == true)
                {
                    return this.Color != null ? Color.ProductImageBundle : null;
                }
                else
                {
                    return Product.ThumbnailImageBundle != null ? Product.ThumbnailImageBundle : null;
                }
            }
        }

        public string Description
        {
            get
            {
                if (Size != null && Color != null)
                {
                    return this.Product.Description + " - " + this.Color.Name + " - " + this.Size.Name;
                }
                if (Size != null)
                {
                    return this.Product.Description + " - " + this.Size.Name;
                }
                if (Color != null)
                {
                    return this.Product.Description + " - " + this.Color.Name;
                }
                return this.Product.Description;
            }
        }

        public int InStock { get; set; }
        public int Reserved { get; set; }

        public int Available
        {
            get
            {
                return Reserved > InStock ? 0 : InStock - Reserved;
            }
        }

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
                Reserved = 0,
                InStock = 0,
                IsDeleted = false,
                DateInserted = DateTime.Now,
                LastModified = DateTime.Now,
            };

            return productSku;
        }
    }
}