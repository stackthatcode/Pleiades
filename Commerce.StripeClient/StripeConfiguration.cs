using System.Collections;
using System.Configuration;

namespace Commerce.StripeClient
{
    public class StripeConfiguration : ConfigurationSection
    {
        static Hashtable _settings = (Hashtable)ConfigurationManager.GetSection("stripeConfiguration");
        static StripeConfiguration _singleton = new StripeConfiguration();

        public static StripeConfiguration Settings
        {
            get { return _singleton; }
        }

        [ConfigurationProperty("SecretKey", IsRequired = true)]
        public string SecretKey
        {
            get { return (string)_settings["SecretKey"]; }
            set { this["SecretKey"] = value; }
        }

        [ConfigurationProperty("PublishableKey", IsRequired = true)]
        public string PublishableKey
        {
            get { return (string)_settings["PublishableKey"]; }
            set { this["PublishableKey"] = value; }
        }

        [ConfigurationProperty("StripeUrl", IsRequired = true)]
        public string StripeUrl
        {
            get { return (string)_settings["StripeUrl"]; }
            set { this["StripeUrl"] = value; }
        }
    }
}
