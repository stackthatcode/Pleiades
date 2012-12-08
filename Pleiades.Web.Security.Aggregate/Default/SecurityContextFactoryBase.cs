using System.Web.Mvc;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Default
{
    public class SecurityContextFactoryBase : ISecurityContextFactory
    {        
        public virtual SecurityContext Create(AuthorizationContext filterContext, AggregateUser user)
        {
            return new SecurityContext(user)
            {
                AccountLevelRestriction = AccountLevel.NotApplicable,
                AuthorizationZone = AuthorizationZone.Restricted,
                IsPaymentArea = false,
            };
        }
    }
}