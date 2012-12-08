using System;
using System.Web.Mvc;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Rules;
using Commerce.WebUI.Areas.Admin.Controllers;
using Commerce.WebUI.Areas.Public.Controllers;

namespace Commerce.WebUI.Plumbing
{
    public class SecurityContextFactory : ISecurityContextFactory 
    {
        public SecurityContext Create(AuthorizationContext filterContext, AggregateUser user)
        {
            if (filterContext.Controller is AuthController || filterContext.Controller is ProductsController)
            {
                return new SecurityContext(user)
                {
                    AuthorizationZone = AuthorizationZone.Public,
                    AccountLevelRestriction = AccountLevel.NotApplicable,
                    IsPaymentArea = false,
                };
            }

            return new SecurityContext(user)
            {
                AuthorizationZone = AuthorizationZone.Administrative,
                AccountLevelRestriction = AccountLevel.NotApplicable,
                IsPaymentArea = false,
            };
        }
    }
}