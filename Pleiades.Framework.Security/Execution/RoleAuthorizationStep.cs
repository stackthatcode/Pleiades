using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class RoleAuthorizationStep<T> : Step<T> 
            where T : ISecurityRequirementsContext, ISecurityContext
    {
        public override void Execute(T context)
        {
            // Public areas are accessible by everybody
            if (context.AuthorizationZone == AuthorizationZone.Public)
            {
                return;
            }

            // Admins have no barriers
            if (context.User.UserRole.IsAdministrator())
            {
                return;
            }

            // Reject anyone that's in an Admin area that's not an Admin
            if (context.AuthorizationZone == AuthorizationZone.Administrative && context.User.UserRole.IsNotAdministrator())
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDenied;
                context.ExecutionStateValid = false;
                return;
            }

            // Reject anyone that's not Trusted in a Trusted area; solicit for Logon
            if (context.AuthorizationZone == AuthorizationZone.Restricted
                && context.User.UserRole != UserRole.Trusted
                && context.User.UserRole.IsNotAdministrator())
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                context.ExecutionStateValid = false;
                return;
            }

            // No denials, keep stepping!
        }
    }
}
