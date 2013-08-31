using System.Web;

namespace Pleiades.Web.Security.Utility
{
    public static class HttpContextExtensions
    {
        public static string ExtractUserNameFromContext(this HttpContextBase context)
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