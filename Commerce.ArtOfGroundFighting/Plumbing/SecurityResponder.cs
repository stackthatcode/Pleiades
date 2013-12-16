using System.Web.Mvc;
using Pleiades.Web.Security.Default;

namespace ArtOfGroundFighting.Web.Plumbing
{
    public class SecurityResponder : DefaultSecurityHttpResponder
    {
        protected override void AccessDeniedSolicitLogon(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}