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

        public string EncodeSecureInformation(string datum)
        {
            // Add HMAC Encryption
            var hmac = new HMACSHA256(Convert.FromBase64String(_settings.Base64EncodedHashKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(datum));
            return Convert.ToBase64String(hash);
        }

        public bool CheckPassword(string unencodedPassword, PfMembershipUser user)
        {
            // We could make the encryption piece injectible, but this is good enough for now
            var submittedEncodedAttempt = EncodeSecureInformation(unencodedPassword);
            return submittedEncodedAttempt == user.Password;            
        }

        public bool CheckPasswordAnswer(string unencodedAnswer, PfMembershipUser user)
        {
            // We could make the encryption piece injectible, but this is good enough for now
            var submittedEncodedAttempt = EncodeSecureInformation(unencodedAnswer);
            return submittedEncodedAttempt == user.PasswordAnswer;
        }
        
        public string GeneratePassword()
        {
            var password =  Membership.GeneratePassword(_settings.MinRequiredPasswordLength, _settings.MinRequiredNonAlphanumericCharacters);
            return password;
        }

        // If the User has failed too many times within a certain window of time, then Lock them out!
        public void UpdateFailedPasswordAttempts(PfMembershipUser user)
        {
            var windowEnd = user.FailedPasswordAttemptWindowStart.AddSeconds(_settings.PasswordAttemptWindowSeconds);

            if (user.FailedPasswordAttemptCount == 0 || DateTime.Now > windowEnd)
            {
                user.FailedPasswordAttemptCount = 1;
                user.FailedPasswordAttemptWindowStart = DateTime.Now;
            }
            else
            {
                user.FailedPasswordAttemptCount++;

                if (user.FailedPasswordAttemptCount >= _settings.MaxInvalidPasswordAttempts)
                {
                    // Max password attempts have exceeded the failure threshold. Lock out the user.
                    user.IsLockedOut = true;
                    user.LastLockedOutDate = DateTime.Now;
                }
            }
        }

        public void UpdateFailedQuestionAndAnswerAttempts(PfMembershipUser user)
        {
            var windowStart = user.FailedPasswordAnswerAttemptWindowStart;
            var windowEnd = windowStart.AddSeconds(_settings.PasswordAttemptWindowSeconds);

            if (user.FailedPasswordAnswerAttemptCount == 0 || DateTime.Now > windowEnd)
            {
                user.FailedPasswordAnswerAttemptCount = 1;
                user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
            }
            else
            {
                user.FailedPasswordAnswerAttemptCount++;
                if (user.FailedPasswordAnswerAttemptCount >= _settings.MaxInvalidPasswordAttempts)
                {
                    // Max password attempts have exceeded the failure threshold. Lock out the user.
                    user.IsLockedOut = true;
                    user.LastLockedOutDate = DateTime.Now;
                }
            }
        }
    }
}
