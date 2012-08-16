using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Framework.Web.Security.Aspect;

namespace Pleiades.Commerce.WebUI.Plumbing.Security
{
    public class CommerceSecurityCodeResponder : PostbackSecurityResponder
    {
        protected override void AccessDeniedSolicitLogon(System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            filterContext.Result = new RedirectResult(
                FormsAuthentication.LoginUrl + "?returnUrl=" +
                    filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
        }
    }
}
