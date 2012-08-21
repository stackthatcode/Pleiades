﻿using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Commerce.WebUI.Plumbing.Navigation;

namespace Pleiades.Commerce.WebUI.Plumbing.Security
{
    public class MySecurityCodeResponder : DefaultResponder
    {
        protected override void AccessDeniedSolicitLogon(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;

            // TODO: add this to Navigation, or create an extension method
            var route = OutboundNavigation.AdminLogin();
            route["returnUrl"] = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl);
            filterContext.Result = new RedirectToRouteResult(OutboundNavigation.AdminLogin());
        }
    }
}
