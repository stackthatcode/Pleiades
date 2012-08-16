using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.MembershipProvider.Interface;

namespace Pleiades.Framework.Web.Security.Execution.Steps
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
                context.OwnerUser.MembershipUserName, context.OldPassword, context.NewPassword);

            return context;
        }
    }
}