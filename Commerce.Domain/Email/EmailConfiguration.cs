using System.Collections;
using System.Configuration;

namespace Commerce.Application.Email
{
    public class EmailConfiguration : ConfigurationSection
    {
        static Hashtable _settings = (Hashtable)ConfigurationManager.GetSection("emailConfiguration");
        static EmailConfiguration _singleton = new EmailConfiguration();

        public static EmailConfiguration Settings
        {
            get { return _singleton; }
        }

        [ConfigurationProperty("MockServiceEnabled", IsRequired = true)]
        public string MockServiceEnabled
        {
            get { return (string)_settings["MockServiceEnabled"]; }
            set { this["MockServiceEnabled"] = value; }
        }

        [ConfigurationProperty("MockServiceOutputDirectory", IsRequired = true)]
        public string MockServiceOutputDirectory
        {
            get { return (string)_settings["MockServiceOutputDirectory"]; }
            set { this["MockServiceOutputDirectory"] = value; }
        }

        [ConfigurationProperty("SmtpHost", IsRequired = true)]
        public string SmtpHost
        {
            get { return (string)_settings["SmtpHost"]; }
            set { this["SmtpHost"] = value; }
        }

        [ConfigurationProperty("SmtpUserName", IsRequired = true)]
        public string SmtpUserName
        {
            get { return (string)_settings["SmtpUserName"]; }
            set { this["SmtpUserName"] = value; }
        }

        [ConfigurationProperty("SmtpPassword", IsRequired = true)]
        public string SmtpPassword
        {
            get { return (string)_settings["SmtpPassword"]; }
            set { this["SmtpPassword"] = value; }
        }

        [ConfigurationProperty("FromAddress", IsRequired = true)]
        public string FromAddress
        {
            get { return (string)_settings["FromAddress"]; }
            set { this["FromAddress"] = value; }            
        }
    }
}
