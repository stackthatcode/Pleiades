using System;
using System.Web.Mvc;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Commerce.WebUI.Areas.Admin.Controllers;
using Commerce.WebUI.Areas.Public.Controllers;

namespace Commerce.WebUI.Plumbing.Security
{
    // *** TODO: Create a Unit Test for this guy ***

    public class MySystemAuthContextBuilder : ISystemAuthContextBuilder 
    {
        public SystemAuthorizationContextBase Build(AuthorizationContext filterContext)
        {
            if (filterContext.Controller is AuthController || filterContext.Controller is ProductsController)
            {
                return new SystemAuthorizationContextBase(filterContext.HttpContext)
                {
                    AuthorizationZone = AuthorizationZone.Public,
                    AccountLevelRestriction = AccountLevel.NotApplicable,
                    IsPaymentArea = false,
                };
            }

            return new SystemAuthorizationContextBase(filterContext.HttpContext)
            {
                AuthorizationZone = AuthorizationZone.Administrative,
                AccountLevelRestriction = AccountLevel.NotApplicable,
                IsPaymentArea = false,
            };
        }
    }
}