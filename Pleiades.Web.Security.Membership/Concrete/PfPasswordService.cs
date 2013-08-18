using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Concrete
{
    public class PfPasswordService : IPfPasswordService
    {
        private readonly IPfMembershipSettings _settings;

        public PfPasswordService(IPfMembershipSettings settings)
        {
            _settings = settings;
        }
         

        // TODO: flesh this out more
        public bool IsValidPassword(string password)
        {
            if (password.Length < _settings.MinRequiredPasswordLength)
            {
                return false;
            }
            if (password.Count(x => !char.IsLetterOrDigit(x)) < _settings.MinRequiredNonAlphanumericCharacters)
            {
                return false;
            }
            return true;
        }

        public string EncodePassword(string password)
        {
            // Add HMAC Encryption
            var hmac = new HMACSHA256(Convert.FromBase64String(_settings.Base64EncodedHashKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        public bool CheckSecureInformation(string submittedUnencodedAttempt, string encodedChallenge)
        {
            // We could make the encryption piece injectible, but this is good enough for now
            var submittedEncodedAttempt = EncodePassword(submittedUnencodedAttempt);
            return submittedEncodedAttempt == encodedChallenge;
        }
        
        public string GeneratePassword()
        {
            var password =  Membership.GeneratePassword(_settings.MinRequiredPasswordLength, _settings.MinRequiredNonAlphanumericCharacters);
            return password;
        }

        // If the User has failed too many times within a certain windows, then Lock them out!
        public void UpdateFailedPasswordAttempts(PfMembershipUser user)
        {
            var failureCount = user.FailedPasswordAttemptCount;
            var windowStart = user.FailedPasswordAttemptWindowStart;
            var windowEnd = windowStart.AddSeconds(_settings.PasswordAttemptWindowSeconds);

            if (failureCount == 0 || DateTime.Now > windowEnd)
            {
                user.FailedPasswordAttemptCount = 1;
                user.FailedPasswordAttemptWindowStart = DateTime.Now;
            }
            else
            {
                // FIXED: failure count
                failureCount++;
                if (failureCount >= _settings.MaxInvalidPasswordAttempts)
                {
                    // Max password attempts have exceeded the failure threshold. Lock out the user.
                    user.IsLockedOut = true;
                    user.LastLockedOutDate = DateTime.Now;
                    user.FailedPasswordAttemptCount = _settings.MaxInvalidPasswordAttempts;
                }
                else
                {
                    user.FailedPasswordAttemptCount = failureCount;
                }
            }
        }

        public void UpdateFailedQuestionAndAnswerAttempts(PfMembershipUser user)
        {
            var failureCount = user.FailedPasswordAnswerAttemptCount;
            var windowStart = user.FailedPasswordAnswerAttemptWindowStart;
            var windowEnd = windowStart.AddSeconds(_settings.PasswordAttemptWindowSeconds);

            if (failureCount == 0 || DateTime.Now > windowEnd)
            {
                user.FailedPasswordAnswerAttemptCount = 1;
                user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
            }
            else
            {
                // FIXED: failure count
                failureCount++;
                if (failureCount >= _settings.MaxInvalidPasswordAttempts)
                {
                    // Max password attempts have exceeded the failure threshold. Lock out the user.
                    user.IsLockedOut = true;
                    user.LastLockedOutDate = DateTime.Now;
                    user.FailedPasswordAnswerAttemptCount = _settings.MaxInvalidPasswordAttempts;
                }
                else
                {
                    user.FailedPasswordAnswerAttemptCount = failureCount;
                }
            }
        }
    }
}
