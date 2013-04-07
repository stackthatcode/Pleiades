using Commerce.Persist.Model.Billing;

namespace Commerce.Persist.Interfaces
{
    public interface IPaymentProcessor
    {
        PaymentProcessorResponse AuthorizeAndCollect(BillingInfo billing, decimal Amount);
    }
}
