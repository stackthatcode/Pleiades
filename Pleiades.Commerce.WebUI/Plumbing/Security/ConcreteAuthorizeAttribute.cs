using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Web.Security.Attributes;

namespace Pleiades.Commerce.WebUI.Plumbing.Security
{
    public class ConcreteAuthorizeAttribute : AuthorizeAttributeBase
    {
        public override void AccessDenied(System.Web.Mvc.AuthorizationContext filterContext)
        {
            AccessDeniedSolicitLogon(filterContext);
        }

        public override void AccessDeniedSolicitLogon(System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(
                System.Web.Security.FormsAuthentication.LoginUrl + "?returnUrl=" +
                    filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
            filterContext.HttpContext.Response.StatusCode = 302;
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}
