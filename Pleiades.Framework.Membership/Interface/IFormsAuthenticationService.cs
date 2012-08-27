using System;
using System.Web.Security;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
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