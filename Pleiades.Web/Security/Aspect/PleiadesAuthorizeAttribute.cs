using System;
using System.Web.Mvc;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Security;
using Pleiades.Framework.Web.Security;
using Pleiades.Framework.Web.Security.Execution.Composites;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PleiadesAuthorizeAttribute : AuthorizeAttribute
    {
        public StepComposite<SystemAuthorizationContextBase> AuthorizeExecution { get; set; }
        public PostbackSecurityResponder Responder { get; set; }
        public PleiadesAuthorizeRule ContextRule { get; set; }

        // System Authorization stuff
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }


        public PleiadesAuthorizeAttribute(
                StepComposite<SystemAuthorizationContextBase> authorizeExecution, 
                PostbackSecurityResponder responder,
                PleiadesAuthorizeRule contextRule)
        {
            this.AuthorizeExecution = authorizeExecution;
            this.Responder = responder;
            this.ContextRule = contextRule;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!this.ContextRule.MustAuthorize(filterContext)) return;
            
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