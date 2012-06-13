using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Security.Execution
{
    public class AccountStatusAuthorizationStep<T> : Step<T>
            where T : ISecurityRequirementsContext, ISecurityContext
    {
        public override void Execute(T context)
        {
            if (context.User.UserRole.IsAdministrator())
            {
                return;
            }

            if (context.AuthorizationZone != AuthorizationZone.Restricted)
            {
                return;
            }

            if (context.User.AccountStatus == null)
            {
                return;
            }

            if (context.User.AccountStatus == AccountStatus.Disabled)
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                context.ExecutionStateValid = false;
            }

            if (context.User.AccountStatus == AccountStatus.PaymentRequired && context.PaymentArea != true)
            {
                context.SecurityResponseCode = SecurityResponseCode.AccountDisabledNonPayment;
                context.ExecutionStateValid = false;
            }

            // Keep stepping!
            return;
        }
    }
}
