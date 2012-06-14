using System;
using System.Web.Security;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Framework.MembershipProvider.Concrete
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SetAuthCookieForUser(string username, bool persistent)
        {
            FormsAuthentication.SetAuthCookie(username, persistent);
        }

        public void ClearAuthenticationCookie()
        {
            FormsAuthentication.SignOut();
        }
    }
}
