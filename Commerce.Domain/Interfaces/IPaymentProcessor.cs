using Commerce.Application.Model.Billing;

namespace Commerce.Application.Interfaces
{
    public interface IPaymentProcessor
    {
        Transaction AuthorizeAndCollect(BillingInfo billing, decimal Amount);
        Transaction Refund(Transaction originalTransaction, decimal Amount);
    }
}
