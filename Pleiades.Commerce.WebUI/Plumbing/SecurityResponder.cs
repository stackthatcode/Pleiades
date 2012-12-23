using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Web.Security.Default;
using Pleiades.Web.Security.Interface;

namespace Commerce.WebUI.Plumbing
{
    public class SecurityResponder : SecurityResponderBase
    {
        protected override void AccessDeniedSolicitLogon(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            var route = AdminNavigation.Login();
            route["returnUrl"] = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl);
            filterContext.Result = new RedirectToRouteResult(AdminNavigation.Login());
        }
    }
}