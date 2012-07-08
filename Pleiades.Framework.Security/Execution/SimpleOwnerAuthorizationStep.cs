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
    public class SimpleOwnerAuthorizationStep<T> : Step<T> where T : IIdentityAuthorizationContext
    {
        public override void Execute(T context)
        {
            if (context.CurrentUser.UserRole.IsAdministrator())
            {
                return;
            }

            if (context.ResourceOwnerId == null)
            {
                return;
            }

            if (context.CurrentUser.ID == context.ResourceOwnerId)
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