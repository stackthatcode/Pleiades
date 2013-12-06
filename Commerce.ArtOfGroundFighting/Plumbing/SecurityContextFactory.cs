using System.Security;
using System.Web.Mvc;
using Commerce.ArtOfGroundFighting.Controllers;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using SecurityContext = Pleiades.Web.Security.Rules.SecurityContext;

namespace Commerce.ArtOfGroundFighting.Plumbing
{
    public class SecurityContextFactory : ISecurityContextBuilder 
    {
        public SecurityContext Create(AuthorizationContext filterContext, AggregateUser user)
        {
            if (filterContext.Controller is ProductsController ||
                filterContext.Controller is PageController || 
                filterContext.Controller is ImageController ||
                filterContext.Controller is CartController ||
                filterContext.Controller is ListController ||
                filterContext.Controller is OrderController)
            {
                return new SecurityContext(user)
                {
                    AuthorizationZone = AuthorizationZone.Public,
                    AccountLevelRestriction = AccountLevel.NotApplicable,
                    IsPaymentArea = false,
                };
            }

            throw new SecurityException(
                "User Attempted to access unauthorized Controller: " + filterContext.Controller.GetType());
        }
    }
}
