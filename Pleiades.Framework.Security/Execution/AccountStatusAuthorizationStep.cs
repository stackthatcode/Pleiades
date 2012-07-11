using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class AccountStatusAuthorizationStep<T> : Step<T> where T : ISystemAuthorizationContext
    {
        public override void Execute(T context)
        {
            if (context.CurrentIdentity.UserRole.IsAdministrator())
            {
                return;
            }

            if (context.AuthorizationZone == AuthorizationZone.Public)
            {
                return;
            }

            if (context.CurrentIdentity.AccountStatus == null)
            {
                this.Kill(context, 
                    () =>
                    {
                        context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                    });
            }

            if (context.CurrentIdentity.AccountStatus == AccountStatus.Disabled)
            {
                this.Kill(context,
                    () =>
                    {
                        context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                    });
            }

            if (context.CurrentIdentity.AccountStatus == AccountStatus.PaymentRequired && 
                context.IsPaymentArea != true)
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
