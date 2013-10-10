using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Model.Billing;
using Commerce.Application.Model.Orders;

namespace Commerce.Application.Model.Cart
{
    public class Cart
    {
        public ShippingMethod Billing { get; set; }
        public List<CartItem> CartItems { get; private set; } 


        public decimal SubTotal
        {
            get
            {
                return CartItems.Sum(x => x.Quantity * x.Sku.Product.UnitPrice);
            }
        }

        public decimal Tax
        {
            get
            {
                
            }
        }


        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }
}
