using Pleiades.Execution;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;

namespace Pleiades.Web.Security.Execution.Step
{
    public class GetUserFromHttpContextStep : Step<SystemAuthorizationContext>
    {
        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IFormsAuthenticationService FormsAuthenticationService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public IHttpContextUserService HttpContextUserService { get; set; }

        public GetUserFromHttpContextStep(
                IAggregateUserRepository aggregateUserRepository, 
                IFormsAuthenticationService formsAuthenticationService,
                IMembershipService membershipService,
                IHttpContextUserService httpContextUserService)
        {
            this.AggregateUserRepository = aggregateUserRepository;
            this.FormsAuthenticationService = formsAuthenticationService;
            this.MembershipService = membershipService;
            this.HttpContextUserService = httpContextUserService;
        }

        public override SystemAuthorizationContext Execute(SystemAuthorizationContext context)
        {
            var userName = context.HttpContext.AuthenticatedUserName();

            if (userName == null)
            {
                context.CurrentUser = AggregateUser.AnonymousFactory();
                return context;
            }

            context.CurrentUser = this.AggregateUserRepository.RetrieveByMembershipUserName(userName);

            if (context.CurrentUser == null)
            {
                this.FormsAuthenticationService.ClearAuthenticationCookie();
                context.CurrentUser = AggregateUser.AnonymousFactory();
                return context;
            }

            this.HttpContextUserService.PutCurrentUserInHttpContext(context.CurrentUser);
            this.MembershipService.Touch(userName);

            return context;
        }
    }
}