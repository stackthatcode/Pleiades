using System;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Execution
{
    public class CompositeIdentityAuthStep<T> : StepComposite<T>
            where T : ISecurityRequirementsContext, ISecurityContext
    {
        public CompositeIdentityAuthStep()
        {
            this.Add(new AccountStatusAuthorizationStep<T>());
            this.Add(new RoleAuthorizationStep<T>());
            this.Add(new SimpleOwnerAuthorizationStep<T>());
            this.Add(new AccountLevelStep<T>());
        }
    }
}
