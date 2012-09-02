using System;
using System.Web.Mvc;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Execution.Context;
using Commerce.WebUI.Areas.Admin.Controllers;
using Commerce.WebUI.Areas.Public.Controllers;

namespace Commerce.WebUI.Plumbing.Security
{
    public class MySystemAuthContextBuilder : ISystemAuthorizationContextBuilder 
    {
        public SystemAuthorizationContext Build(AuthorizationContext filterContext)
        {
            if (filterContext.Controller is AuthController || filterContext.Controller is ProductsController)
            {
                return new SystemAuthorizationContext(filterContext.HttpContext)
                {
                    AuthorizationZone = AuthorizationZone.Public,
                    AccountLevelRestriction = AccountLevel.NotApplicable,
                    IsPaymentArea = false,
                };
            }

            return new SystemAuthorizationContext(filterContext.HttpContext)
            {
                AuthorizationZone = AuthorizationZone.Administrative,
                AccountLevelRestriction = AccountLevel.NotApplicable,
                IsPaymentArea = false,
            };
        }
    }
}