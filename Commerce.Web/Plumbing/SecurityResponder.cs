using System.Web.Mvc;
using Pleiades.Web.Security.Default;

namespace Commerce.Web.Plumbing
{
    public class SecurityResponder : DefaultSecurityHttpResponder
    {
        protected override void AccessDeniedSolicitLogon(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            var route = Navigation.Login();
            route["returnUrl"] = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl);
            filterContext.Result = new RedirectToRouteResult(route);
        }
    }
}