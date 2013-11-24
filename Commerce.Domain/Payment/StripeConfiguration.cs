using System.Collections;
using System.Configuration;

namespace Commerce.Application.Payment
{
    public class StripeConfiguration : ConfigurationSection
    {
        static readonly Hashtable _settings = (Hashtable)ConfigurationManager.GetSection("stripeConfiguration");
        static readonly StripeConfiguration _singleton = new StripeConfiguration();

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

        [ConfigurationProperty("ClientSideMockEnabled", IsRequired = true)]
        public string ClientSideMockEnabled
        {
            get { return (string)_settings["ClientSideMockEnabled"]; }
            set { this["ClientSideMockEnabled"] = value; }
        }

        [ConfigurationProperty("ServerSideMockEnabled", IsRequired = true)]
        public string ServerSideMockEnabled
        {
            get { return (string) _settings["ServerSideMockEnabled"]; }
            set { this["ServerSideMockEnabled"] = value; }
        }
    }
}
