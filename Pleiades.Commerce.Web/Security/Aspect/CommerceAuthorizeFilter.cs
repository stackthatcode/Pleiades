using System;
using System.Web.Mvc;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Web.Security;
using Pleiades.Commerce.Web.Security.Concrete;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;


namespace Pleiades.Commerce.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CommerceAuthorizeAttribute : AuthorizeAttribute
    {
        public AuthorizeFromFilterComposite AuthorizeExecution { get; set; }
        public CommerceSecurityCodeResponder Responder { get; set; }

        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }


        public CommerceAuthorizeAttribute(
                AuthorizeFromFilterComposite authorizeExecution, CommerceSecurityCodeResponder responder)
        {
            this.AuthorizeExecution = authorizeExecution;
            this.Responder = responder;
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
            
            this.AuthorizeExecution.Execute(context);
            this.Responder.ProcessSecurityCode(context.SecurityResponseCode, filterContext);
        }
    }
}
