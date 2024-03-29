using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Billing;
using Commerce.Application.Orders.Entities;

namespace Commerce.Application.Shopping.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid CartIdentifier { get; set;  }
        public List<CartItem> CartItems { get; private set; }
        public ShippingMethod ShippingMethod { get; set; }
        public StateTax StateTax { get; set; }
        public Total Total { get; set; }

        public string OrderExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModified { get; set; }

        private Func<decimal> SubTotal
        {
            get
            {
                return () => CartItems.Sum(x => x.Quantity * x.Sku.Product.UnitPrice);
            }
        }

        public bool OrderSubmitted
        {
            get { return OrderExternalId != null; }
        }

        public Cart()
        {
            Total = new Total(SubTotal, () => ShippingMethod, () => StateTax);
            CartItems = new List<CartItem>();
            CreatedOn = DateTime.Now;
            LastModified = DateTime.Now;
        }
    }
}
