using System;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Concrete
{
    public class PfMembershipSettings : IPfMembershipSettings 
    {
        // Add stuff to read the values from configuration

        public int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public int PasswordAttemptWindowSeconds
        {
            get { throw new NotImplementedException(); }
        }

        public int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public string Base64EncodedHashKey
        {
            get { throw new NotImplementedException(); }
        }

        public bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public TimeSpan UserIsOnlineTimeWindow
        {
            get { throw new NotImplementedException(); }
        }
    }
}
