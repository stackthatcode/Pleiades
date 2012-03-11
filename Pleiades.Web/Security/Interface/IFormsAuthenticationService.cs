using System;
using System.Web.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IFormsAuthenticationService
    {
        void SetAuthCookieForUser(DomainUser user, bool persistent);
        void ClearAuthenticationCookie();
    }
}