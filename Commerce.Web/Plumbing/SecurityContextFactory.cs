using System.Web.Mvc;
using Commerce.Web.Areas.Public.Controllers;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Rules;

namespace Commerce.Web.Plumbing
{
    public class SecurityContextFactory : ISecurityContextBuilder 
    {
        public SecurityContext Create(AuthorizationContext filterContext, AggregateUser user)
        {
            if (filterContext.Controller is ProductsController ||
                filterContext.Controller is PageController || 
                filterContext.Controller is ImageController ||
                filterContext.Controller is CartController ||
                filterContext.Controller is ListController)
            {
                return new SecurityContext(user)
                {
                    AuthorizationZone = AuthorizationZone.Public,
                    AccountLevelRestriction = AccountLevel.NotApplicable,
                    IsPaymentArea = false,
                };
            }

            if (filterContext.Controller is Areas.Admin.Controllers.UnsecuredController)
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
