namespace Commerce.StripeClient
{
    public class ConfigurationAdapter : IConfigurationAdapter
    {
        public string SecretKey
        {
            get { return StripeConfiguration.Settings.SecretKey; }
        }

        public string PublishableKey
        {
            get { return StripeConfiguration.Settings.PublishableKey; }
        }

        public string StripeUrl
        {
            get { return StripeConfiguration.Settings.StripeUrl; }
        }
    }
}
