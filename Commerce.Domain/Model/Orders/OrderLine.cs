using System;
using Commerce.Persist.Model.Products;

namespace Commerce.Persist.Model.Orders
{
    public class OrderLine
    {
        public int Id { get; set; }
        public ProductSku Sku { get; set; }
        public string OriginalSkuCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal LinePrice
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }
    }
}