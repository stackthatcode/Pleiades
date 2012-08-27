using System;
using Pleiades.Execution;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Steps
{
    public class AuthenticateUserByRoleStep : Step<AuthenticateUserByRoleContext>
    {
        public IMembershipService MembershipService { get; set; }
        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IFormsAuthenticationService FormsAuthService { get; set; }

        public AuthenticateUserByRoleStep(
                IMembershipService membershipService,
                IAggregateUserRepository aggregateUserRepository, 
                IFormsAuthenticationService formsAuthService)
        {
            this.FormsAuthService = formsAuthService;
            this.AggregateUserRepository = aggregateUserRepository;
            this.MembershipService = membershipService;
        }

        public override AuthenticateUserByRoleContext Execute(AuthenticateUserByRoleContext context)
        {
            var membershipUser = 
                this.MembershipService.ValidateUserByEmailAddr(context.AttemptedUserName, context.AttemptedPassword);

            if (membershipUser == null)
            {
                this.FormsAuthService.ClearAuthenticationCookie();
                context.Message = "Invalid Credentials";
                return this.Kill(context);
            }

            var aggregateuser = this.AggregateUserRepository.RetrieveByMembershipUserName(membershipUser.UserName);

            if (!context.ExpectedRoles.Contains(aggregateuser.IdentityUser.UserRole))
            {
                this.FormsAuthService.ClearAuthenticationCookie();
                context.Message = "Invalid Role";
                return this.Kill(context);
            }

            // Success!
            this.FormsAuthService.SetAuthCookieForUser(membershipUser.UserName, context.PersistenceCookie);
            return context;
        }
    }
}
