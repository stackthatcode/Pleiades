using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Pleiades.Web.Security.Model
{
    /// <summary>
    /// Pleiades Framework representation of MembershipProvider User object
    /// </summary>
    public class MembershipUser
    {
        // Membership Identifiers
        public string UserName { get; private set; }
        public object ProviderUserKey { get; private set; }

        // Properties are externally-manipulable through interface
        public string Email { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }

        // Membership Provider state properties
        public DateTime CreationDate { get; private set; }
        public bool IsLockedOut { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastActivityDate { get; private set; }
        public DateTime LastLoginDate { get; private set; }
        public string PasswordQuestion { get; set; }


        /// <summary>
        /// Transfers from MembershipUser to Pleiades MembershipUser
        /// </summary>
        public static Model.MembershipUser ConvertToPleiades(System.Web.Security.MembershipUser user)
        {
            return new MembershipUser
            {
                UserName = user.UserName,
                ProviderUserKey = user.ProviderUserKey,

                Email = user.Email,
                Comment = user.Comment,
                IsApproved = user.IsApproved,

                CreationDate = user.CreationDate,
                IsLockedOut = user.IsLockedOut,
                IsOnline = user.IsOnline,
                LastActivityDate = user.LastActivityDate,
                LastLoginDate = user.LastLoginDate,
                PasswordQuestion = user.PasswordQuestion,
            };
        }
    }
}
