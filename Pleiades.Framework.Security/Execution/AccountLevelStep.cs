using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class AccountLevelStep<T> : Step<T>
            where T : IIdentityRequirementsContext, ISecurityContext, IIdentityUserContext
    {
        public override void Execute(T context)
        {
            if (context.IdentityUser.UserRole.IsAdministrator())
            {
                return; 
            }

            // Only applies to Restricted Areas
            if (context.IdentityRequirements.AuthorizationZone != AuthorizationZone.Restricted)
            {
                return;
            }

            if (context.IdentityRequirements.AccountLevelRestriction == AccountLevel.Standard)
            {
                return;
            }

            if (context.IdentityRequirements.AccountLevelRestriction == AccountLevel.Gold && 
                context.IdentityUser.AccountLevel != AccountLevel.Gold)
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
