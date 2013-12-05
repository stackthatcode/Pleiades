using System;
using System.Web.Mvc;
using Pleiades.App.Data;
using Pleiades.App.Injection;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Aspect
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SecurityAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // TODO: refactor for testability that confirms invocation of extension methods (???)

            var _container = DependencyResolver.Current.GetService<IContainerAdapter>();
            var contextFactory = _container.Resolve<ISecurityContextBuilder>();
            var httpSecurityResponder = _container.Resolve<ISecurityHttpResponder>();
            var aggregateUserService = _container.Resolve<IAggregateUserService>();
            var unitOfWork = _container.Resolve<IUnitOfWork>();

            var user = aggregateUserService.LoadAuthentedUserIntoContext(filterContext.HttpContext);

            // For the touch method
            unitOfWork.SaveChanges();

            var context = 
                contextFactory.Create(filterContext, user)
                    .AccountLevelCheck()
                    .AccountStatusCheck()
                    .UserRoleCheck();
            
            httpSecurityResponder.Process(context.SecurityCode, filterContext);
        }
    }
}