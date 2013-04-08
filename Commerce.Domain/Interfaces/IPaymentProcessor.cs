using Commerce.Persist.Model.Billing;

namespace Commerce.Persist.Interfaces
{
    public interface IPaymentProcessor
    {
        ProcessorResponse AuthorizeAndCollect(BillingInfo billing, decimal Amount);
    }
}
