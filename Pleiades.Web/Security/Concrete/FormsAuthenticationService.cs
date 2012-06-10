using System;
using System.Web.Security;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Concrete
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SetAuthCookieForUser(DomainUser user, bool persistent)
        {
            FormsAuthentication.SetAuthCookie(user.MembershipUser.UserName, persistent);
        }

        public void ClearAuthenticationCookie()
        {
            FormsAuthentication.SignOut();
        }
    }
}
