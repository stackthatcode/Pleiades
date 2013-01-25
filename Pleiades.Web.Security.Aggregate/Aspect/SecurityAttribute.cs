using System;
using System.Diagnostics;
using System.Web.Mvc;
using Pleiades.Injection;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SecurityAttribute : AuthorizeAttribute
    {        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //
            // TODO: refactor for testability that confirms invocation of extension methods (???)
            //

            var _container = DependencyResolver.Current.GetService<IContainerAdapter>();

            var contextFactory = _container.Resolve<ISecurityContextBuilder>();
            var httpSecurityResponder = _container.Resolve<ISecurityHttpResponder>();
            var aggregateUserService = _container.Resolve<IAggregateUserService>();
            Debug.WriteLine("AggrUserService Id: " + aggregateUserService.Tracer);

            var user = aggregateUserService.GetAuthenticatedUser(filterContext.HttpContext);

            var context = 
                contextFactory.Create(filterContext, user)
                    .AccountLevelCheck()
                    .AccountStatusCheck()
                    .UserRoleCheck();
            
            httpSecurityResponder.Process(context.SecurityCode, filterContext);
        }
    }
}