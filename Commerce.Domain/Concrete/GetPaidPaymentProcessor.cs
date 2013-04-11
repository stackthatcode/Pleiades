using System;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Billing;

namespace Commerce.Persist.Concrete
{
    public class GetPaidPaymentProcessor : IPaymentProcessor
    {
        // The various Payment Processor responses get decoded and/or logged/persisted in this implementation
        public Transaction AuthorizeAndCollect(BillingInfo billing, decimal amount)
        {
            // If somebody's payment fails... then Log it.
            var pretendReferenceCode = Guid.NewGuid().ToString();

            return new Transaction
            {
                Amount = amount,
                TransactionType = TransactionType.AuthorizeAndCollect,                

                // Stubbed out Stripe details
                ProcessorCode = "PAID-3333",
                Details = "Payment accepted",
                ReferenceCode = pretendReferenceCode,
                OriginalReferenceCode = pretendReferenceCode,
                Success = true,
            };
        }

        public Transaction Refund(Transaction originalTransaction, decimal amount)
        {
            return new Transaction
            {
                Amount = amount,
                TransactionType = TransactionType.Refund,
                OriginalReferenceCode = originalTransaction.ReferenceCode,

                // Stubbed out Stripe details
                ProcessorCode = "REFUND-7777",
                Details = "Refund granted",
                ReferenceCode = Guid.NewGuid().ToString(),
                Success = true,
            };
        }
    }
}
