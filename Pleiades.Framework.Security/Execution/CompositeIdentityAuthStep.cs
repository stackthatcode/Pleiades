using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Identity.Execution
{
    public class CompositeIdentityAuthStep<T> : StepComposite<T>
            where T : IIdentityRequirementsContext, ISecurityContext, IIdentityUserContext
    {
        public CompositeIdentityAuthStep(Container container) : base(container)
        {
            this.Inject<AccountStatusAuthorizationStep<T>>();
            this.Inject<RoleAuthorizationStep<T>>();
            this.Inject<SimpleOwnerAuthorizationStep<T>>();
            this.Inject<AccountLevelStep<T>>();
        }
    }
}
