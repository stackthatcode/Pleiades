using System;
using Commerce.Persist.Model.Products;

namespace Commerce.Persist.Model.Orders
{
    public class OrderLine
    {
        public OrderLine(ProductSku sku, int quantity)
        {
            this.Sku = sku;
            this.OriginalSkuCode = sku.SkuCode;
            this.OriginalDescription = sku.Description;
            this.OriginalUnitPrice = sku.Product.UnitPrice;
            this.Quantity = quantity;
        }

        public int Id { get; set; }

        // This guy may or may not exist...
        public ProductSku Sku { get; set; }
        
        public string OriginalSkuCode { get; set; }
        public string OriginalDescription { get; set; }
        public decimal OriginalUnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal LinePrice
        {
            get
            {
                return Quantity * OriginalUnitPrice;
            }
        }
    }
}