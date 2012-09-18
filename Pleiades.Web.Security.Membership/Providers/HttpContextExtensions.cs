using System.Web;
using System.Security.Principal;

namespace Pleiades.Web.Security.Providers
{
    public static class HttpContextExtensions
    {
        public static string AuthenticatedUserName(this HttpContextBase context)
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