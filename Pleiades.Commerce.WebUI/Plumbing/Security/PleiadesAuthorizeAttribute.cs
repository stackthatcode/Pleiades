using System;
using System.Web.Mvc;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PleiadesAuthorizeAttribute : AuthorizeAttribute
    {
        public ISystemAuthorizationContextBuilder ContextBuilder { get; set; }
        public IStep<ISystemAuthorizationContext> AuthorizeExecution { get; set; }


        public IPostbackSecurityResponder Responder { get; set; }

        public PleiadesAuthorizeAttribute(IGenericContainer container)
        {
            this.ContextBuilder = container.ResolveAuthContextBuilder();
            this.AuthorizeExecution = container.ResolveSystemAuthorizationStep();


            this.Responder = container.ResolvePostbackResponder();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            var context = this.ContextBuilder.Build(filterContext);
            this.AuthorizeExecution.Execute(context);


            this.Responder.Execute(context.SecurityResponseCode, filterContext);
        }
    }
}