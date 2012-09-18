using System;
using System.Web.Mvc;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Security;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Composites;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SystemAuthorizeAttribute : AuthorizeAttribute
    {
        // Injected by downstream consumers of my stuff
        public ISystemAuthorizationContextBuilder ContextBuilder { get; set; }
        public IPostbackSecurityResponder Responder { get; set; }
        public SystemAuthorizationComposite AuthorizeExecution { get; set; }

        public SystemAuthorizeAttribute(
                ISystemAuthorizationContextBuilder contextBuilder,
                IPostbackSecurityResponder responder,
                SystemAuthorizationComposite authorizeExecution)
        {
            this.ContextBuilder = contextBuilder;
            this.Responder = responder;
            this.AuthorizeExecution = authorizeExecution;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            var context = this.ContextBuilder.Build(filterContext);
            this.AuthorizeExecution.Execute(context);
            this.Responder.Execute(context.SecurityResponseCode, filterContext);
        }
    }
}