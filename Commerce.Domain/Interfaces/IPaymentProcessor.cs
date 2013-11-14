using Commerce.Application.Model.Billing;

namespace Commerce.Application.Interfaces
{
    public interface IPaymentProcessor
    {
        Transaction Charge(string token, decimal amount);
        Transaction Refund(Transaction originalTransaction, decimal amount);
    }
}
