using System.Web.Security;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Concrete
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