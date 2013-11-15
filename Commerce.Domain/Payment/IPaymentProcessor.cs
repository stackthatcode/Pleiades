using Commerce.Application.Billing;

namespace Commerce.Application.Payment
{
    public interface IPaymentProcessor
    {
        Transaction Charge(string token, decimal amount);
        Transaction Refund(Transaction originalTransaction, decimal amount);
    }
}
