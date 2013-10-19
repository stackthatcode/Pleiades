using System;
using Commerce.Application.Model.Orders;
using Newtonsoft.Json;

namespace Commerce.Application.Model.Billing
{
    public class Total
    {
        [JsonIgnore]
        private readonly Func<decimal> _subTotal;

        public int Id { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public StateTax StateTax { get; set; }

        public Total(Func<decimal> subTotal)
        {
            _subTotal = subTotal;
        }

        public decimal SubTotal 
        { 
            get { return _subTotal(); }
        }

        public decimal Tax
        {
            get
            {
                if (StateTax == null)
                {
                    return 0;
                }
                return SubTotal * StateTax.TaxRate / 100.00m;
            }
        }

        public decimal ShippingCost
        {
            get
            {
                return (SubTotal != 0 && ShippingMethod != null) ? ShippingMethod.Cost : 0;
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return SubTotal + Tax + ShippingCost;
            }
        }
    }
}
