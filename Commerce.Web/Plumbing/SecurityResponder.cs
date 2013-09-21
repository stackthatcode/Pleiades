using System.Web.Mvc;
using System.Web.Security;
using Commerce.Web.Areas.Admin;
using Pleiades.Web.Security.Default;
using Pleiades.Web.Security.Interface;

namespace Commerce.Web.Plumbing
{
    public class SecurityResponder : DefaultSecurityHttpResponder
    {
        protected override void AccessDeniedSolicitLogon(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            var route = AdminNavigation.Login();
            route["returnUrl"] = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl);
            filterContext.Result = new RedirectToRouteResult(route);
        }
    }
}