namespace Commerce.Application.Concrete.Payment
{
    public interface IConfigurationAdapter
    {
        string SecretKey { get; }
        string PublishableKey { get; }
        string StripeUrl { get; }
    }
}
