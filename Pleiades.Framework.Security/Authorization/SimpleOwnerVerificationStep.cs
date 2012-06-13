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
    public class OwnerAuthorizationStep<T> : Step<T> 
            where T : ISecurityRequirementsContext, ISecurityContext
    {
        public override void Execute(T context)
        {
            if (context.User.UserRole.IsAdministrator())
            {
                return;
            }

            if (context.User.ID == context.ResourceOwnerDomainUserId)
            {
                return;
            }

            context.SecurityResponseCode = SecurityResponseCode.AccessDenied;
            context.ExecutionStateValid = false;
        }
    }
}