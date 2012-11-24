using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Web.Security.Default;
using Pleiades.Web.Security.Interface;
using Commerce.WebUI.Plumbing.Navigation;

namespace Commerce.WebUI.Plumbing.Security
{
    public class CommerceSecurityResponder : SecurityResponder
    {
        protected override void AccessDeniedSolicitLogon(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            var route = OutboundNavigation.AdminLogin();
            route["returnUrl"] = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl);
            filterContext.Result = new RedirectToRouteResult(OutboundNavigation.AdminLogin());
        }
    }
}