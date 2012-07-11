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
    public class SimpleOwnerAuthorizationStep<T> : Step<T> where T : IOwnerAuthorizationContext
    {
        public override void Execute(T context)
        {
            if (context.CurrentUserIdentity.UserRole.IsAdministrator())
            {
                return;
            }

            if (context.OwnerIdentityId == null)
            {
                return;
            }

            if (context.CurrentUserIdentity.ID == context.OwnerIdentityId)
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