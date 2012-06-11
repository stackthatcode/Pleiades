using System;
using Model = Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Providers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// A helper function that takes the current persistent user and creates a MembershiUser from the values.
        /// </summary>
        /// <param name="user">user object containing the user data retrieved from database</param>
        /// <returns>membership user object</returns>
        public static System.Web.Security.MembershipUser ToSecurityMembershipUser(this Model.MembershipUser user, string providerName)
        {
            return new 
                System.Web.Security.MembershipUser(
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

    }
}
