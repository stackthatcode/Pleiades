using System;
using System.Web.Security;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Framework.MembershipProvider.Interface
{
    /// <summary>
    /// Enables us to abstract and mock the Forms Authentication functions
    /// </summary>
    public interface IFormsAuthenticationService
    {
        void SetAuthCookieForUser(string username, bool persistent);
        void ClearAuthenticationCookie();
    }
}