using Commerce.Application.Model.Billing;

namespace Commerce.Application.Interfaces
{
    public interface IPaymentsProcessor
    {
        Transaction Charge(string token, decimal amount);
        Transaction Refund(Transaction originalTransaction, decimal amount);
    }
}
