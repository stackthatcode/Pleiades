using System;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Billing;

namespace Commerce.Application.Concrete.Infrastructure
{
    public class GetPaidPaymentProcessor : IPaymentsService
    {
        // The various Payment Processor responses get decoded and/or logged/persisted in this implementation
        public Transaction AuthorizeAndCollect(BillingInfo billing, decimal amount)
        {
            // If somebody's payment fails... then Log it.
            var pretendReferenceCode = Guid.NewGuid().ToString();

            return new Transaction(TransactionType.AuthorizeAndCollect)
            {
                Amount = amount,

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
            return new Transaction(TransactionType.Refund)
            {
                Amount = amount,
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
