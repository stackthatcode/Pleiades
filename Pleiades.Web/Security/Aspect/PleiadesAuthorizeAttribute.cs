using System;
using System.Web.Mvc;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Execution.Composites;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PleiadesAuthorizeAttribute : AuthorizeAttribute
    {
        public ISystemAuthContextBuilder ContextBuilder { get; set; }
        public StepComposite<SystemAuthorizationContextBase> AuthorizeExecution { get; set; }
        public IPostbackSecurityResponder Responder { get; set; }

        // System Authorization stuff
        //public AuthorizationZone AuthorizationZone { get; set; }
        //public AccountLevel AccountLevelRestriction { get; set; }
        //public bool IsPaymentArea { get; set; }


        public PleiadesAuthorizeAttribute(IGenericContainer container)
        {
            this.AuthorizeExecution = 
                container.ResolveKeyed<StepComposite<SystemAuthorizationContextBase>>(
                    FrameworkWebModule.SystemAuthorizationStepKey);
            this.Responder = container.Resolve<IPostbackSecurityResponder>();
            this.ContextBuilder = container.Resolve<ISystemAuthContextBuilder>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            var context = this.ContextBuilder.Build(filterContext);

            this.AuthorizeExecution.Execute(context);
            this.Responder.Execute(context.SecurityResponseCode, filterContext);
        }
    }
}