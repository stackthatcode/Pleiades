using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Billing;

namespace Commerce.Persist.Concrete
{
    public class GetPaidPaymentProcessor : IPaymentProcessor
    {
        // The various Payment Processor responses get decoded and/or logged/persisted in this implementation
        public PaymentProcessorResponse AuthorizeAndCollect(BillingInfo billing, decimal Amount)
        {
            // If somebody's payment fails... then what...?  Log it...?

            return new PaymentProcessorResponse
            {
                 ProcessorCode = "ABC-12345",
                 Details = "",
                 //Unified
                 Success = true,
            };
        }
    }
}
