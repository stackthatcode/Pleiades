using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class AccountStatusAuthorizationStep<T> : Step<T>
            where T : IIdentityRequirementsContext, ISecurityContext, IIdentityUserContext
    {
        public override void Execute(T context)
        {
            if (context.IdentityUser.UserRole.IsAdministrator())
            {
                return;
            }

            if (context.IdentityRequirements.AuthorizationZone == AuthorizationZone.Public)
            {
                return;
            }

            if (context.IdentityUser.AccountStatus == null)
            {
                this.Kill(context, 
                    () =>
                    {
                        context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                    });
            }

            if (context.IdentityUser.AccountStatus == AccountStatus.Disabled)
            {
                this.Kill(context,
                    () =>
                    {
                        context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                    });
            }

            if (context.IdentityUser.AccountStatus == AccountStatus.PaymentRequired && 
                context.IdentityRequirements.PaymentArea != true)
            {
                this.Kill(context,
                    () =>
                    {
                        context.SecurityResponseCode = SecurityResponseCode.AccountDisabledNonPayment;
                    });
            }

            // Keep stepping!
            return;
        }
    }
}
