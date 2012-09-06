using System.Web.Mvc;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Concrete
{
    public class DefaultAuthorizationContextBuilder : ISystemAuthorizationContextBuilder
    {
        public virtual SystemAuthorizationContext Build(AuthorizationContext filterContext)
        {
            return new SystemAuthorizationContext(filterContext.HttpContext)
            {
                AccountLevelRestriction = AccountLevel.NotApplicable,
                AuthorizationZone = AuthorizationZone.Restricted,
                IsPaymentArea = false,
            };
        }
    }
}