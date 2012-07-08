﻿using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class RoleAuthorizationStep<T> : Step<T> where T : IIdentityAuthorizationContext
    {
        public override void Execute(T context)
        {
            // Public areas are accessible by everybody
            if (context.AuthorizationZone == AuthorizationZone.Public)
            {
                return;
            }

            // Admins have no barriers
            if (context.CurrentUser.UserRole.IsAdministrator())
            {
                return;
            }

            // Reject anyone that's in an Admin area that's not an Admin
            if (context.AuthorizationZone == AuthorizationZone.Administrative && 
                context.CurrentUser.UserRole.IsNotAdministrator())
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDenied;
                this.Kill(context);
                return;
            }

            // Reject anyone that's not Trusted in a Trusted area; solicit for Logon
            if (context.AuthorizationZone == AuthorizationZone.Restricted
                && context.CurrentUser.UserRole != UserRole.Trusted
                && context.CurrentUser.UserRole.IsNotAdministrator())
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDeniedSolicitLogon;
                this.Kill(context);
                return;
            }

            // No denials, keep stepping!
        }
    }
}
