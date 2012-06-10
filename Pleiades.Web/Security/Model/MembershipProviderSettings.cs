using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Pleiades.Web.Security
{
    /// <summary>
    /// Isolates Membership Provider Settings to a separate object
    /// </summary>
	public class MembershipProviderSettings
	{
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

        public static MembershipProviderSettings Extract(MembershipProvider provider)
        {
            return new MembershipProviderSettings
            {
                MaxInvalidPasswordAttempts = provider.MaxInvalidPasswordAttempts,
                PasswordAttemptWindow = provider.PasswordAttemptWindow,
                MinRequiredPasswordLength = provider.MinRequiredPasswordLength,
                MinRequiredNonAlphanumericCharacters = provider.MinRequiredNonAlphanumericCharacters,
                PasswordStrengthRegularExpression = provider.PasswordStrengthRegularExpression,
                PasswordFormat = provider.PasswordFormat,
                EnablePasswordReset = provider.EnablePasswordReset,
                EnablePasswordRetrieval = provider.EnablePasswordRetrieval,
                RequiresQuestionAndAnswer = provider.RequiresQuestionAndAnswer,
                RequiresUniqueEmail = provider.RequiresUniqueEmail,
            };
        }
	}
}