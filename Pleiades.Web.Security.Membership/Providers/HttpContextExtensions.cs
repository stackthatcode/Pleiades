using System.Web;
using System.Security.Principal;

namespace Pleiades.Web.Security.Providers
{
    public static class HttpContextExtensions
    {
        public static string MembershipUserName(this HttpContextBase context)
        {
            var identity = context.User.Identity;

            if (identity == null || identity.IsAuthenticated == false)
            {
                return null;
            }
            else
            {
                return identity.Name;
            }
        }
    }
}