﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Pleiades.Framework.Web.Security.Model
{
    /// <summary>
    /// Membership User maps ASP.NET Membership Provider concerns to a seperate object
    /// </summary>
    public class MembershipUser
    {
        // Membership Identifiers
        public object ProviderUserKey { get; set; }
        public string UserName { get; set; }

        // Membership-Provider specific Data
        public string ApplicationName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Password { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsApproved { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime LastLockedOutDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public int FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public int FailedPasswordAnswerAttemptWindowStart { get; set; }

        // Properties are externally-manipulable through interface
        public string Comment { get; set; }

        // Membership Provider state properties
        public DateTime CreationDate { get; private set; }
        public DateTime LastModified { get; private set; }


        /// <summary>
        /// Transfers from MembershipUser to Pleiades MembershipUser
        /// 
        /// TODO: replace this entirely with AutoMapper
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
