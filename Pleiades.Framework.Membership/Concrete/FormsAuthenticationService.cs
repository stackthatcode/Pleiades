using System.Web.Security;
using Pleiades.Framework.MembershipProvider.Interface;

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
