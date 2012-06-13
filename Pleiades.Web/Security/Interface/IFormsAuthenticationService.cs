using System;
using System.Web.Security;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Interface
{
    public interface IFormsAuthenticationService
    {
        void SetAuthCookieForUser(string username, bool persistent);
        void ClearAuthenticationCookie();
    }
}