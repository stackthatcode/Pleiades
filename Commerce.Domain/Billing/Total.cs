using System;
using Commerce.Application.Orders.Entities;
using Newtonsoft.Json;

namespace Commerce.Application.Billing
{
    public class Total
    {
        [JsonIgnore]
        private readonly Func<decimal> _subTotal;
        private readonly Func<ShippingMethod> _shippingMethod;
        private readonly Func<StateTax> _stateTax;

        public int Id { get; set; }

        public Total(Func<decimal> subTotal, Func<ShippingMethod> shippingMethod, Func<StateTax> stateTax)
        {
            _subTotal = subTotal;
            _shippingMethod = shippingMethod;
            _stateTax = stateTax;
        }

        public decimal SubTotal 
        { 
            get { return _subTotal(); }
        }

        public decimal Tax
        {
            get
            {
                if (_stateTax == null || _stateTax() == null)
                {
                    return 0;
                }
                return SubTotal * _stateTax().TaxRate / 100.00m;
            }
        }

        public decimal SubTotalInclTax
        {
            get { return SubTotal + Tax; }
        }

        public decimal ShippingCost
        {
            get
            {
                return (_shippingMethod != null && _shippingMethod() != null) ? _shippingMethod().Cost : 0;
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return SubTotalInclTax + ShippingCost;
            }
        }
    }
}
