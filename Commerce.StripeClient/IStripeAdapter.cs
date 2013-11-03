namespace Commerce.StripeClient
{
    public interface IStripeAdapter
    {
        string MakePayment();
        string MakeRefund();
    }
}
