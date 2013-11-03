using System.Configuration;

namespace Commerce.StripeClient
{
    public class StripeConfiguration : ConfigurationSection
    {
        private static 
            StripeConfiguration _settings
                = ConfigurationManager.GetSection("stripeConfiguration") as StripeConfiguration;

        public static StripeConfiguration Settings
        {
            get { return _settings; }
        }

        [ConfigurationProperty("SecretKey", IsRequired = true)]
        public string SecretKey
        {
            get { return (string)this["SecretKey"]; }
            set { this["SecretKey"] = value; }
        }

        [ConfigurationProperty("PublishableKey", IsRequired = true)]
        public string PublishableKey
        {
            get { return (string)this["PublishableKey"]; }
            set { this["PublishableKey"] = value; }
        }

        [ConfigurationProperty("StripeUrl", IsRequired = true)]
        public string StripeUrl
        {
            get { return (string)this["StripeUrl"]; }
            set { this["StripeUrl"] = value; }
        }
    }
}
