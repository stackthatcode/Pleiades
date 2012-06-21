using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;

namespace Pleiades.Framework.MembershipProvider.Providers
{
    /// <summary>
    /// Isolates Membership Provider Settings
    /// </summary>
	public class MembershipProviderSettings
	{
        public string ApplicationName { get; set; }
        public MachineKeySection MachineKey { get; private set; }
        public string ConnectionString { get; private set; }

        public int MaxInvalidPasswordAttempts { get; private set; }
        public int PasswordAttemptWindow { get; private set; }
        public int MinRequiredPasswordLength { get; private set; }
        public int MinRequiredNonAlphanumericCharacters { get; private set; }
        public string PasswordStrengthRegularExpression { get; private set; }
        public MembershipPasswordFormat PasswordFormat { get; private set; }
        public bool EnablePasswordReset { get; private set; }
        public bool EnablePasswordRetrieval { get; private set; }
        public bool RequiresQuestionAndAnswer { get; private set; }
        public bool RequiresUniqueEmail { get; private set; }
        public TimeSpan UserIsOnlineTimeWindow { get; private set; }

        public MembershipProviderSettings(NameValueCollection config)
        {
            // TODO: wire this properly into parsing the configuration
            this.UserIsOnlineTimeWindow = new TimeSpan(0, 15, 0);

            this.MaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            this.PasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            this.MinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            this.MinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            this.PasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            
            this.EnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            this.EnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            this.RequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            this.RequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            
            this.ApplicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);

            var ConnectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }
            this.ConnectionString = ConnectionStringSettings.ConnectionString;            

            string temp_format = config["passwordFormat"] ?? "Hashed";
            switch (temp_format)
            {
                case "Hashed":
                    this.PasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    this.PasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    this.PasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Get encryption and decryption key information from the configuration.
            Configuration cfg;
            if (HostingEnvironment.IsHosted)
            {
                cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
                MachineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");
            }
            else
            {
                MachineKey = (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");
            }

            if (MachineKey.ValidationKey.Contains("AutoGenerate"))
            {
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.");
            }
        }
        
        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetConfigValue(string configValue, string defaultValue)
        {
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }
	}
}