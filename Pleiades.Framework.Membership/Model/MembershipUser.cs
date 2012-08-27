using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Pleiades.Web.Security.Model
{
    /// <summary>
    /// Membership User maps ASP.NET Membership Provider concerns to a seperate object
    /// </summary>
    public class MembershipUser
    {
        // For EF's sake
        public int Id { get; set; }

        // Membership Identifiers
        public object ProviderUserKey { get; set; }
        public string UserName { get; set; }

        // Membership-Provider specific Data
        public string ApplicationName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsApproved { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public bool IsOnline { get; set; }
        public bool? IsLockedOut { get; set; }
        public DateTime LastLockedOutDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

        // Properties are externally-manipulable through interface
        public string Comment { get; set; }

        // Membership Provider state properties
        public DateTime CreationDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
