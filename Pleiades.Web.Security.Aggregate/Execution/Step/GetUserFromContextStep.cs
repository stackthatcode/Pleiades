using Pleiades.Execution;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;

namespace Pleiades.Web.Security.Execution.Step
{
    public class GetUserFromContextStep : Step<SystemAuthorizationContext>
    {
        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IFormsAuthenticationService FormsAuthenticationService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public GetUserFromContextStep(
                IAggregateUserRepository aggregateUserRepository, 
                IFormsAuthenticationService formsAuthenticationService,
                IMembershipService membershipService)
        {
            this.AggregateUserRepository = aggregateUserRepository;
            this.FormsAuthenticationService = formsAuthenticationService;
            this.MembershipService = membershipService;
        }

        public override SystemAuthorizationContext Execute(SystemAuthorizationContext context)
        {
            var userName = context.HttpContext.AuthenticatedUserName();
            if (userName == null)
            {
                context.ThisUser = AggregateUser.AnonymousUserFactory();
                return context;
            }

            context.ThisUser = this.AggregateUserRepository.RetrieveByMembershipUserName(userName);

            if (context.ThisUser == null)
            {
                this.FormsAuthenticationService.ClearAuthenticationCookie();
                context.ThisUser = AggregateUser.AnonymousUserFactory();
                return context;
            }

            this.MembershipService.Touch(userName);
            return context;
        }
    }
}