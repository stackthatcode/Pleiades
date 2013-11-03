using Commerce.Application.Model.Billing;

namespace Commerce.Application.Interfaces
{
    public interface IPaymentsService
    {
        Transaction AuthorizeAndCollect(BillingInfo billing, decimal amount);
        Transaction Refund(Transaction originalTransaction, decimal amount);
    }
}
