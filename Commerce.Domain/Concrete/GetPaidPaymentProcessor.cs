using System;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Billing;

namespace Commerce.Persist.Concrete
{
    public class GetPaidPaymentProcessor : IPaymentProcessor
    {
        // The various Payment Processor responses get decoded and/or logged/persisted in this implementation
        public ProcessorResponse AuthorizeAndCollect(BillingInfo billing, decimal Amount)
        {
            // If somebody's payment fails... then Log it.
            
            return new ProcessorResponse
            {
                 ProcessorCode = "ABC-12345",
                 Details = "Payment accepted",
                 ExternalReferenceCode = Guid.NewGuid().ToString(),
                 Success = true,
            };
        }
    }
}
