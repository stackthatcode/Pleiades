using System;
using Pleiades.Execution;
using Pleiades.Security;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Step
{
    public class RoleAuthorizationStep : Step<SystemAuthorizationContext>
    {
        public override SystemAuthorizationContext Execute(SystemAuthorizationContext context)
        {
            // Public areas are accessible by everybody
            if (context.AuthorizationZone == AuthorizationZone.Public)
            {
                return context;
            }

            // Admins have no barriers
            if (context.CurrentUser.IdentityProfile.UserRole.IsAdministratorOrSupreme())
            {
                return context;
            }

            // Reject anyone that's in an Admin area that's not an Admin
            if (context.AuthorizationZone == AuthorizationZone.Administrative && 
                context.CurrentUser.IdentityProfile.UserRole.IsNotAdministratorOrSupreme())
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDenied;
                this.Kill(context);
                return context;
            }

            // Reject anyone that's not Trusted in a Trusted area; solicit for Logon
            if (context.AuthorizationZone == AuthorizationZone.Restricted
                && context.CurrentUser.IdentityProfile.UserRole != UserRole.Trusted
                && context.CurrentUser.IdentityProfile.UserRole.IsNotAdministratorOrSupreme())
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                return this.Kill(context);
            }

            // No denials, keep stepping!
            return context;
        }
    }
}
