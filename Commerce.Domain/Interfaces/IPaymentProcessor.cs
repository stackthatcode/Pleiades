using Commerce.Persist.Model.Billing;

namespace Commerce.Persist.Interfaces
{
    public interface IPaymentProcessor
    {
        Transaction AuthorizeAndCollect(BillingInfo billing, decimal Amount);
        Transaction Refund(Transaction originalTransaction, decimal Amount);
    }
}
