using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Providers;

namespace Pleiades.Framework.Web.Security.Execution.Steps
{
    public class GetUserFromContextStep : Step<SystemAuthorizationContextBase>
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

        public override SystemAuthorizationContextBase Execute(SystemAuthorizationContextBase context)
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
