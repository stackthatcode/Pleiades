using System;
using System.Web.Mvc;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Commerce.WebUI.Areas.Admin.Controllers;
using Pleiades.Commerce.WebUI.Areas.Public.Controllers;

namespace Pleiades.Commerce.WebUI.Plumbing.Security
{
    // *** TODO: Create a Unit Test for this guy ***

    public class MySystemAuthContextBuilder : ISystemAuthContextBuilder 
    {
        public SystemAuthorizationContextBase Build(AuthorizationContext filterContext)
        {
            if (filterContext.Controller is LoginController ||
                filterContext.Controller is ProductsController)
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