using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Security.Authorization
{
    public class AccountLevelStep<T> : Step<T>
            where T : ISecurityRequirementsContext, ISecurityContext
    {
        public override void Execute(T context)
        {
            if (context.User.UserRole.IsAdministrator())
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

            if (context.AccountLevelRestriction == AccountLevel.Gold && context.User.AccountLevel != AccountLevel.Gold)
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDeniedToAccountLevel;
            }

            return;
        }
    }
}
