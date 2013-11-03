using System;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Billing;

namespace Commerce.Application.Concrete.Payment
{
    public class StripePaymentProcessor : IPaymentsProcessor
    {
        public Transaction Charge(string token, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Transaction Refund(Transaction originalTransaction, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
