using System;
using System.Configuration;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Concrete
{
    public class PfMembershipSettings : IPfMembershipSettings 
    {
        // Add stuff to read the values from configuration

        public int MaxInvalidPasswordAttempts
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["MaxInvalidPasswordAttempts"]); }
        }

        public int PasswordAttemptWindowSeconds
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["PasswordAttemptWindowSeconds"]); }
        }

        public int MinRequiredPasswordLength
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["MinRequiredPasswordLength"]); }
        }

        public int MinRequiredNonAlphanumericCharacters
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["MinRequiredNonAlphanumericCharacters"]); }
        }

        public string PasswordStrengthRegularExpression
        {
            get { return ConfigurationManager.AppSettings["PasswordStrengthRegularExpression"]; }
        }

        public string Base64EncodedHashKey
        {
            get { return ConfigurationManager.AppSettings["Base64EncodedHashKey"]; }
        }

        public bool EnablePasswordReset
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["EnablePasswordReset"]); }
        }

        public bool RequiresQuestionAndAnswer
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["RequiresQuestionAndAnswer"]); }
        }

        public TimeSpan UserIsOnlineTimeWindow
        {
            get 
            { 
                return new TimeSpan(0, 0, Int32.Parse(ConfigurationManager.AppSettings["UserIsOnlineTimeWindow"])); 
            }
        }
    }
}
