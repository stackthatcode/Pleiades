namespace Commerce.StripeClient
{
    public interface IConfigurationAdapter
    {
        string SecretKey { get; }
        string PublishableKey { get; }
        string StripeUrl { get; }
    }
}
