using System;
using Model = Pleiades.Framework.MembershipProvider.Model;
using WebSecurity = System.Web.Security;

namespace Pleiades.Framework.MembershipProvider.Providers
{
    public static class MembershipExtensions
    {
        /// <summary>
        /// A helper function that takes the current persistent user and creates a MembershiUser from the values.
        /// </summary>
        /// <param name="user">user object containing the user data retrieved from database</param>
        /// <returns>membership user object</returns>
        public static WebSecurity.MembershipUser ToSecurityMembershipUser(this Model.MembershipUser user, string providerName)
        {
            return new
                WebSecurity.MembershipUser(
                        providerName,
                        user.UserName,
                        user.ProviderUserKey,
                        user.Email,
                        user.PasswordQuestion,
                        user.Comment,
                        user.IsApproved,
                        Convert.ToBoolean(user.IsLockedOut),
                        Convert.ToDateTime(user.CreationDate),
                        Convert.ToDateTime(user.LastLoginDate),
                        Convert.ToDateTime(user.LastActivityDate),
                        Convert.ToDateTime(user.LastPasswordChangedDate),
                        Convert.ToDateTime(user.LastLockedOutDate));
        }

        /// <summary>
        /// Transfers from MembershipUser to Pleiades MembershipUser
        /// 
        /// TODO: replace this entirely with AutoMapper
        /// </summary>
        public static Model.MembershipUser ToModelMembershipUser(this System.Web.Security.MembershipUser user)
        {
            if (user == null) return null;
            
            return new Model.MembershipUser
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
