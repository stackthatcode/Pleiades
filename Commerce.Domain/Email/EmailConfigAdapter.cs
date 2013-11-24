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

        [ConfigurationProperty("ServerSideMockEnabled", IsRequired = true)]
        public string ServerSideMockEnabled
        {
            get { return (string)_settings["ServerSideMockEnabled"]; }
            set { this["ServerSideMockEnabled"] = value; }
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

        [ConfigurationProperty("SmtpPort", IsRequired = true)]
        public string SmtpPort
        {
            get { return (string)_settings["SmtpPort"]; }
            set { this["SmtpPort"] = value; }
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
