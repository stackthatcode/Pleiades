using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Persist.Model.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public ShippingInfo ShippingInfo { get; set; }
        public List<OrderLine> OrderLines { get; set; }

        // TODO: create a Tax Rate table
        public decimal TaxRate { get; set; }

        // Computed properties...
        public decimal SubTotal
        {
            get
            {
                return OrderLines.Sum(x => x.LinePrice);
            }
        }

        public decimal Tax
        {
            get
            {
                return SubTotal * TaxRate;
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return SubTotal + (SubTotal * TaxRate) + ShippingInfo.ShippingCost;
            }
        }
    }
}