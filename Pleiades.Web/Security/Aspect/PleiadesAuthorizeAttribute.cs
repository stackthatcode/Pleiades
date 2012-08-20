using System;
using System.Web.Mvc;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Security;
using Pleiades.Framework.Web.Security;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Execution.Composites;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PleiadesAuthorizeAttribute : AuthorizeAttribute
    {
        public StepComposite<SystemAuthorizationContextBase> AuthorizeExecution { get; set; }
        public IPostbackSecurityResponder Responder { get; set; }
        public IAuthorizationRule AuthorizationRule { get; set; }

        // System Authorization stuff
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }


        public PleiadesAuthorizeAttribute(IGenericContainer container)
        {
            this.AuthorizeExecution = 
                container.ResolveKeyed<StepComposite<SystemAuthorizationContextBase>>(
                    FrameworkWebModule.SystemAuthorizationStepKey);
            this.Responder = container.Resolve<IPostbackSecurityResponder>();
            this.AuthorizationRule = container.Resolve<IAuthorizationRule>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!this.AuthorizationRule.MustAuthorize(filterContext)) return;
            
            var context = 
                new SystemAuthorizationContextBase()
                {
                    HttpContext = filterContext.HttpContext,
                    AuthorizationZone = this.AuthorizationZone,
                    AccountLevelRestriction = this.AccountLevelRestriction,
                    IsPaymentArea = this.IsPaymentArea,
                };
            
            this.AuthorizeExecution.Execute(context);
            this.Responder.Execute(context.SecurityResponseCode, filterContext);
        }
    }
}