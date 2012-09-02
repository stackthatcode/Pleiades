using System;
using Pleiades.Execution;
using Pleiades.Security;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Step
{
    public class AccountStatusAuthorizationStep : Step<SystemAuthorizationContext>
    {
        public override SystemAuthorizationContext Execute(SystemAuthorizationContext context)
        {
            if (context.ThisUser.IdentityProfile.UserRole.IsAdministrator())
            {
                return context;
            }

            if (context.AuthorizationZone == AuthorizationZone.Public)
            {
                return context;
            }

            if (context.ThisUser.IdentityProfile.AccountStatus == null)
            {
                return 
                    this.Kill(context, 
                        () =>
                        {
                            context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                        });
            }

            if (context.ThisUser.IdentityProfile.AccountStatus == AccountStatus.Disabled)
            {
                return 
                    this.Kill(context,
                        () =>
                        {
                            context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                        });
            }

            if (context.ThisUser.IdentityProfile.AccountStatus == AccountStatus.PaymentRequired && 
                context.IsPaymentArea != true)
            {
                return 
                    this.Kill(context,
                        () =>
                        {
                            context.SecurityResponseCode = SecurityResponseCode.AccountDisabledNonPayment;
                        });
            }

            // Keep stepping!
            return context;
        }
    }
}
