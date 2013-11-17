namespace Commerce.Application.Payment
{
    public interface IStripeConfigAdapter
    {
        string SecretKey { get; }
        string PublishableKey { get; }
        string StripeUrl { get; }
    }
}
