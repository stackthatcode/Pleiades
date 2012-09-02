using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Commerce.WebUI.Plumbing.Navigation;

namespace Commerce.WebUI.Plumbing.Security
{
    public class MySecurityCodeResponder : DefaultPostbackSecurityResponder
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