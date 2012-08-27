using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Execution;
using Pleiades.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution
{
    public class SimpleOwnerAuthorizationStep<T> : Step<T> where T : IOwnerAuthorizationContext
    {
        public override T Execute(T context)
        {
            if (context.CurrentUserIdentity.UserRole.IsAdministrator())
            {
                return context;
            }

            if (context.OwnerIdentityId == null)
            {
                return context;
            }

            if (context.CurrentUserIdentity.ID == context.OwnerIdentityId)
            {
                return context;
            }

            return 
                this.Kill(context, () =>
                {
                    context.SecurityResponseCode = SecurityResponseCode.AccessDenied;
                });
        }
    }
}