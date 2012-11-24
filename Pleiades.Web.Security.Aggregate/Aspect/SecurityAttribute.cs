using System;
using System.Web.Mvc;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SecurityAttribute : AuthorizeAttribute
    {
        // Injected by upstream consumers of my stuff
        public ISecurityContextFactory ContextFactory { get; set; }
        public IAggregateUserService AggregateUserService { get; set; }
        public IHttpSecurityResponder HttpSecurityResponder { get; set; }

        public SecurityAttribute(
                ISecurityContextFactory contextFactory, 
                IAggregateUserService aggregateUserService,
                IHttpSecurityResponder responder)
        {
            this.ContextFactory = contextFactory;
            this.AggregateUserService = aggregateUserService;
            this.HttpSecurityResponder = responder;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //
            // TODO: refactor for testability that confirms invocation of extension methods
            //
            var user = this.AggregateUserService.GetAuthenticatedUser(filterContext.HttpContext);
            var context = this.ContextFactory.Create(filterContext, user);

            context.AccountLevelCheck();
            context.AccountStatusCheck();
            context.UserRoleCheck();            
            this.HttpSecurityResponder.Process(context.SecurityCode, filterContext);
        }
    }
}