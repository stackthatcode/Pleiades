using System;
using Commerce.Application.Billing;

namespace Commerce.Application.Payment
{
    public class MockPaymentProcessor : IPaymentProcessor
    {
        // The various Payment Processor responses get decoded and/or logged/persisted in this implementation
        public Transaction Charge(string token, decimal amount)
        {
            // If somebody's payment fails... then Log it.
            var pretendReferenceCode = Guid.NewGuid().ToString();

            return new Transaction(TransactionType.AuthorizeAndCollect)
            {
                Amount = amount,

                // Stubbed out Stripe details
                ProcessorResponse = "PAID-3333",
                Details = "Payment accepted",
                ReferenceCode = pretendReferenceCode,
                OriginalReferenceCode = pretendReferenceCode,
                Success = true,
            };
        }

        public Transaction Refund(Transaction originalTransaction, decimal amount)
        {
            return new Transaction(TransactionType.Refund)
            {
                Amount = amount,
                OriginalReferenceCode = originalTransaction.ReferenceCode,
                ReferenceCode = Guid.NewGuid().ToString(),
                
                // Stubbed out Stripe details
                ProcessorResponse = "REFUND-7777",
                Details = "Refund granted",
                Success = true,
            };
        }
    }
}
