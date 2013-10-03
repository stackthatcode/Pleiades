using System;
using Commerce.Application.Model.Resources;

namespace Commerce.Application.Model.Products
{
    public class ProductSku
    {
        public int Id { get; set; }        
        public string SkuCode { get; set; }
        public string OriginalSkuCode { get; set; }

        public Product Product { get; set; }
        public ProductColor Color { get; set; }
        public ProductSize Size { get; set; }

        public string Name
        {
            get
            {
                if (Size != null && Color != null)
                {
                    return this.Product.Name + " - " + this.Color.Name + " - " + this.Size.Name;
                }
                if (Size != null)
                {
                    return this.Product.Name + " - " + this.Size.Name;
                }
                if (Color != null)
                {
                    return this.Product.Name + " - " + this.Color.Name;
                }

                return this.Product.Name;
            }
        }

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

            if (color != null && size != null)
            {
                skuCode += "-" + color.SkuCode + "-" + size.SkuCode;
                name += " - " + color.Name + " - " + size.Name;
            }
            if (color != null && size == null)
            {
                skuCode += "-" + color.SkuCode;
                name += " - " + color.Name; 
            }
            if (size != null && color == null)
            {
                skuCode += "-" + size.SkuCode;
                name += " - " + size.Name;
            }

            var productSku = new ProductSku
            {
                OriginalSkuCode = skuCode,
                
                SkuCode = skuCode,
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
