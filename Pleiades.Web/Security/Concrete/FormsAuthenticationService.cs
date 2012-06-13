using System;
using System.Web.Security;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Concrete
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


        // TODO: wire these in
            //        var user = DomainUserService.RetrieveUserByEmail(emailaddr);
            //if (user == null)
            //{
            //    return null;
            //}

            //if (user.AccountStatus == Model.AccountStatus.Disabled)
            //{
            //    return null;
            //}

    }
}
