using Pleiades.Commerce.Domain.Interface;
using Pleiades.Commerce.Domain.Entities.Users;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Providers;

namespace Pleiades.Commerce.Web.Security.Execution.Authorization
{
    public class GetUserFromHttpContextStep : Step<AggrUserAuthContext>
    {
        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IFormsAuthenticationService FormsAuthenticationService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public GetUserFromHttpContextStep(
                IAggregateUserRepository aggregateUserRepository, 
                IFormsAuthenticationService formsAuthenticationService,
                IMembershipService membershipService)
        {
            this.AggregateUserRepository = aggregateUserRepository;
            this.FormsAuthenticationService = formsAuthenticationService;
            this.MembershipService = membershipService;
        }

        public override void Execute(AggrUserAuthContext context)
        {
            var userName = context.HttpContext.AuthenticatedUserName();
            if (userName == null)
            {
                context.AggregateUser = AggregateUser.AnonymousUserFactory();
                return;
            }

            context.AggregateUser = this.AggregateUserRepository.RetrieveUserByMembershipUserName(userName);

            if (context.AggregateUser == null)
            {
                this.FormsAuthenticationService.ClearAuthenticationCookie();
                context.AggregateUser = AggregateUser.AnonymousUserFactory();
                return;
            }

            this.MembershipService.Touch(userName);
        }
    }
}
