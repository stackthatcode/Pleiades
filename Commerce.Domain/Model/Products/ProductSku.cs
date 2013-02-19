using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Model.Products
{
    public class ProductSku
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }        
        public string SkuCode { get; set; }   
        public int TotalInventory { get; set; }
        public int ReservedInventory { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime LastModified { get; set; }

        public string AutoGenSkuCode()
        {
            string skuCode = Product.SkuCode;
            if (Color != null)
                skuCode += "-" + Color.SkuCode;
            if (Size != null)
                skuCode += "-" + Size.SkuCode;
            return skuCode;
        }
    }
}