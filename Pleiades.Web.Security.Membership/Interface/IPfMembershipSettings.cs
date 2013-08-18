using System;

namespace Pleiades.Web.Security.Interface
{
    public interface IPfMembershipSettings
    {
        int MaxInvalidPasswordAttempts { get; }
        int PasswordAttemptWindowSeconds { get; }
        int MinRequiredPasswordLength { get; }
        int MinRequiredNonAlphanumericCharacters { get; }
        string PasswordStrengthRegularExpression { get; }
        string Base64EncodedHashKey { get; }
        bool EnablePasswordReset { get; }
        bool RequiresQuestionAndAnswer { get; }
        TimeSpan UserIsOnlineTimeWindow { get; }
    }
}
