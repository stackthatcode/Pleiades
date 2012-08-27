using System;
using Pleiades.Execution;
using Pleiades.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution
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
