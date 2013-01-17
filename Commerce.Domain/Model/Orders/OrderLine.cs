using System;
using Commerce.Domain.Model.Products;

namespace Commerce.Domain.Model.Orders
{
    public class OrderLine
    {
        public int Id { get; set; }
        public Sku Sku { get; set; }
        public string OriginalSkuCode { get; set; }

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