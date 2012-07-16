using System;
using System.Web.Mvc;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Web.Security;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CommerceAuthorizeAttribute : AuthorizeAttribute
    {
        public IGenericContainer Container { get; set; }

        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }


        public CommerceAuthorizeAttribute(IGenericContainer container)
        {
            this.Container = container;
        }

        public void  OnAuthorization(AuthorizationContext filterContext)
        {
            var context = 
                new SystemAuthorizationContextBase()
                {
                    HttpContext = filterContext.HttpContext,
                    AuthorizationZone = this.AuthorizationZone,
                    AccountLevelRestriction = this.AccountLevelRestriction,
                    IsPaymentArea = this.IsPaymentArea,
                };
            
            var execution = this.Container.Resolve<AuthorizeFromFilterComposite>();
            execution.Execute(context);

            var response = new SecurityCodeFilterResponder();
            response.ProcessSecurityCode(context.SecurityResponseCode, filterContext);
        }
    }
}
