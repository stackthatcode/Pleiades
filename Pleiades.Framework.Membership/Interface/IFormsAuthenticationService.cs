using System;
using System.Web.Security;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Framework.MembershipProvider.Interface
{
    public interface IFormsAuthenticationService
    {
        void SetAuthCookieForUser(string username, bool persistent);
        void ClearAuthenticationCookie();
    }
}