using System.Web.Mvc;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Rules;
using Commerce.Web.Areas.Admin.Controllers;
using Commerce.Web.Areas.Public.Controllers;

namespace Commerce.Web.Plumbing
{
    public class SecurityContextFactory : ISecurityContextBuilder 
    {
        public SecurityContext Create(AuthorizationContext filterContext, AggregateUser user)
        {
            if (filterContext.Controller is AuthController || 
                filterContext.Controller is ProductsController || 
                filterContext.Controller is Areas.Admin.Controllers.SystemController ||
                filterContext.Controller is Areas.Public.Controllers.SystemController)
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