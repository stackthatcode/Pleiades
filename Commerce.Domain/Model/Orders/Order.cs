using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Persist.Model.Orders
{
    public class Order
    {
        public int Id { get; set; }
        // public Customer Customer { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public decimal TaxRate { get; set; }

        public decimal TotalBeforeTax
        {
            get
            {
                return OrderLines.Sum(x => x.LinePrice);
            }
        }

        public decimal TotalAfterTax
        {
            get
            {
                return TotalBeforeTax - TotalBeforeTax * TaxRate;
            }
        }
    }
}