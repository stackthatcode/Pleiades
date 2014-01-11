using System.Collections;
using System.Configuration;

namespace Commerce.Application.Azure
{
    public class AzureConfiguration : ConfigurationSection
    {
        static readonly Hashtable _settings =
            (Hashtable)ConfigurationManager.GetSection("azureConfiguration");

        static readonly AzureConfiguration _singleton = new AzureConfiguration();

        public static AzureConfiguration Settings
        {
            get { return _singleton; }
        }

        [ConfigurationProperty("StorageConnectionString", IsRequired = true)]
        public string StorageConnectionString
        {
            get { return (string)_settings["StorageConnectionString"]; }
            set { this["StorageConnectionString"] = value; }
        }

        [ConfigurationProperty("ResourcesStorageContainer", IsRequired = true)]
        public string ResourcesStorageContainer
        {
            get { return (string)_settings["ResourcesStorageContainer"]; }
            set { this["ResourcesStorageContainer"] = value; }
        }
    }
}
