using Pleiades.Commerce.Domain.Interface;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Providers;

namespace Pleiades.Commerce.Web.Security.Execution
{
    public class GetUserFromFilterContextStep : Step<SystemAuthorizationContextBase>
    {
        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IFormsAuthenticationService FormsAuthenticationService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public GetUserFromFilterContextStep(
                IAggregateUserRepository aggregateUserRepository, 
                IFormsAuthenticationService formsAuthenticationService,
                IMembershipService membershipService)
        {
            this.AggregateUserRepository = aggregateUserRepository;
            this.FormsAuthenticationService = formsAuthenticationService;
            this.MembershipService = membershipService;
        }

        public override void Execute(SystemAuthorizationContextBase context)
        {
            var userName = context.HttpContext.AuthenticatedUserName();
            if (userName == null)
            {
                context.ThisUser = AggregateUser.AnonymousUserFactory();
                return;
            }

            context.ThisUser = this.AggregateUserRepository.RetrieveUserByMembershipUserName(userName);

            if (context.ThisUser == null)
            {
                this.FormsAuthenticationService.ClearAuthenticationCookie();
                context.ThisUser = AggregateUser.AnonymousUserFactory();
                return;
            }

            this.MembershipService.Touch(userName);
        }
    }
}
