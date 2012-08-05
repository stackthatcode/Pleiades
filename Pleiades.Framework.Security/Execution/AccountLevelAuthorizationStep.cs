using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class AccountLevelAuthorizationStep<T> : Step<T>
            where T : ISystemAuthorizationContext
    {
        public override T Execute(T context)
        {
            if (context.CurrentIdentity.UserRole.IsAdministrator())
            {
                return context; 
            }

            // Only applies to Restricted Areas
            if (context.AuthorizationZone != AuthorizationZone.Restricted)
            {
                return context;
            }

            if (context.AccountLevelRestriction == AccountLevel.Standard)
            {
                return context;
            }

            if (context.AccountLevelRestriction == AccountLevel.Gold && 
                context.CurrentIdentity.AccountLevel != AccountLevel.Gold)
            {
                return 
                    this.Kill(context, () =>
                        {
                            context.SecurityResponseCode = SecurityResponseCode.AccessDeniedToAccountLevel;
                        });
            }

            return context;
        }
    }
}
