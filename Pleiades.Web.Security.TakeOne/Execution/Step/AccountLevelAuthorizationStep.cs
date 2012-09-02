using System;
using Pleiades.Execution;
using Pleiades.Security;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Step
{
    public class AccountLevelAuthorizationStep : Step<SystemAuthorizationContext>
    {
        public override SystemAuthorizationContext Execute(SystemAuthorizationContext context)
        {
            if (context.ThisUser.IdentityProfile.UserRole.IsAdministrator())
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
                context.ThisUser.IdentityProfile.AccountLevel != AccountLevel.Gold)
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
