using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.MembershipProvider.Interface;

namespace Pleiades.Commerce.Web.Security.Execution.NonPublic
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

        public override void Execute(ChangeUserPasswordContext context)
        {
            this.MembershipService.ChangePassword(
                context.OwnerUser.MembershipUserName, context.OldPassword, context.NewPassword);
        }
    }
}