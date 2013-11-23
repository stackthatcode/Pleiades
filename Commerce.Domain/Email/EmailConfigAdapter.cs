using System.Collections;
using System.Configuration;

namespace Commerce.Application.Email
{
    public class EmailConfigAdapter : ConfigurationSection, IEmailConfigAdapter
    {
        static readonly Hashtable _settings = (Hashtable)ConfigurationManager.GetSection("emailConfiguration");
        static readonly EmailConfigAdapter _singleton = new EmailConfigAdapter();

        public static EmailConfigAdapter Settings
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

        [ConfigurationProperty("CustomerServiceEmail", IsRequired = true)]
        public string CustomerServiceEmail
        {
            get { return (string)_settings["CustomerServiceEmail"]; }
            set { this["CustomerServiceEmail"] = value; }            
        }

        [ConfigurationProperty("SystemEmail", IsRequired = true)]
        public string SystemEmail
        {
            get { return (string)_settings["SystemEmail"]; }
            set { this["SystemEmail"] = value; }
        }

        [ConfigurationProperty("TemplateDirectory", IsRequired = true)]
        public string TemplateDirectory
        {
            get { return (string)_settings["TemplateDirectory"]; }
            set { this["TemplateDirectory"] = value; }            
        }
    }
}
