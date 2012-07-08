using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class AccountLevelAuthorizationStep<T> : Step<T>
            where T : IIdentityAuthorizationContext
    {
        public override void Execute(T context)
        {
            if (context.CurrentUser.UserRole.IsAdministrator())
            {
                return; 
            }

            // Only applies to Restricted Areas
            if (context.AuthorizationZone != AuthorizationZone.Restricted)
            {
                return;
            }

            if (context.AccountLevelRestriction == AccountLevel.Standard)
            {
                return;
            }

            if (context.AccountLevelRestriction == AccountLevel.Gold && 
                context.CurrentUser.AccountLevel != AccountLevel.Gold)
            {
                this.Kill(context,
                    () =>
                    {
                        context.SecurityResponseCode = SecurityResponseCode.AccessDeniedToAccountLevel;
                    });
            }

            return;
        }
    }
}
