using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Model.Billing;

namespace Commerce.Application.Model.Shopping
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid CartIdentifier { get; set;  }
        public List<CartItem> CartItems { get; private set; }
        public Total Total { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModified { get; set; }

        private Func<decimal> SubTotal
        {
            get
            {
                return () => CartItems.Sum(x => x.Quantity * x.Sku.Product.UnitPrice);
            }
        }

        public Cart()
        {
            Total = new Total(SubTotal);
            CartItems = new List<CartItem>();
            CreatedOn = DateTime.Now;
            LastModified = DateTime.Now;
        }
    }
}
