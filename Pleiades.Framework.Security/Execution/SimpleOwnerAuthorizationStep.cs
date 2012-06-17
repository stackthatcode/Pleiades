using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Identity.Execution
{
    public class SimpleOwnerAuthorizationStep<T> : Step<T>
            where T : IIdentityRequirementsContext, ISecurityContext, IIdentityUserContext
    {
        public override void Execute(T context)
        {
            if (context.IdentityUser.UserRole.IsAdministrator())
            {
                return;
            }

            if (context.IdentityRequirements.ResourceOwnerIdentityUserId == null)
            {
                return;
            }

            if (context.IdentityUser.ID == context.IdentityRequirements.ResourceOwnerIdentityUserId)
            {
                return;
            }

            this.Kill(context, () =>
            {
                context.SecurityResponseCode = SecurityResponseCode.AccessDenied;
            });
        }
    }
}