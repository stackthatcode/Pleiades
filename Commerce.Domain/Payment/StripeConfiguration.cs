using System.Collections;
using System.Configuration;

namespace Commerce.Application.Payment
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

        [ConfigurationProperty("ClientSideDebugMode", IsRequired = true)]
        public bool ClientSideDebugMode
        {
            get { return _settings["ClientSideDebugMode"].ToString().ToUpper() == "TRUE"; }
            set { this["ClientSideDebugMode"] = value; }
        }

        [ConfigurationProperty("MockServiceEnabled", IsRequired = true)]
        public bool MockServiceEnabled
        {
            get { return _settings["MockServiceEnabled"].ToString().ToUpper() == "TRUE"; }
            set { this["MockServiceEnabled"] = value; }
        }
    }
}
