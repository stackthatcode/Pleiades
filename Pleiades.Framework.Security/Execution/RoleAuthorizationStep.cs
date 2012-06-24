using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class RoleAuthorizationStep<T> : Step<T>
            where T : IIdentityRequirementsContext, ISecurityContext, IIdentityUserContext
    {
        public override void Execute(T context)
        {
            // Public areas are accessible by everybody
            if (context.IdentityRequirements.AuthorizationZone == AuthorizationZone.Public)
            {
                return;
            }

            // Admins have no barriers
            if (context.IdentityUser.UserRole.IsAdministrator())
            {
                return;
            }

            // Reject anyone that's in an Admin area that's not an Admin
            if (context.IdentityRequirements.AuthorizationZone == AuthorizationZone.Administrative && 
                context.IdentityUser.UserRole.IsNotAdministrator())
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDenied;
                this.Kill(context);
                return;
            }

            // Reject anyone that's not Trusted in a Trusted area; solicit for Logon
            if (context.IdentityRequirements.AuthorizationZone == AuthorizationZone.Restricted
                && context.IdentityUser.UserRole != UserRole.Trusted
                && context.IdentityUser.UserRole.IsNotAdministrator())
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                this.Kill(context);
                return;
            }

            // No denials, keep stepping!
        }
    }
}
