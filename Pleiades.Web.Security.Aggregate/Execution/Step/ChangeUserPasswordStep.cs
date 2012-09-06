using Pleiades.Execution;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Execution.Step
{
    /// <summary>
    /// WARNING: do not construct this object directly.  Please use the ChangeUserPasswordStepFactory
    /// The constructor is made public for white-box Unit Testing.
    /// </summary>
    public class ChangeUserPasswordStep : Step<ChangeUserPasswordContext>
    {
        IMembershipService MembershipService { get; set; }

        public ChangeUserPasswordStep(IMembershipService membershipService)
        {
            this.MembershipService = membershipService;
        }

        public override ChangeUserPasswordContext Execute(ChangeUserPasswordContext context)
        {
            this.MembershipService.ChangePassword(
                context.OwnerUser.Membership.UserName, context.OldPassword, context.NewPassword);

            return context;
        }
    }
}