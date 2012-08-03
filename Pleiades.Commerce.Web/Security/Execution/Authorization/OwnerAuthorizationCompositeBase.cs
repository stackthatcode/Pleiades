using System.Collections.Generic;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Commerce.Web.Security.Execution.Steps;

namespace Pleiades.Commerce.Web.Security.Execution.Authorization
{
    public class OwnerAuthorizationCompositeBase<T> : StepComposite<T>
            where T : OwnerAuthorizationContextBase
    {
        public OwnerAuthorizationCompositeBase(IGenericContainer container, Step<T> guardedSteps)
            : base(container)
        {
            // Authorization Step
            this.Inject<SimpleOwnerAuthorizationStep<T>>();

            // Guarded Steps
            this.Register(guardedSteps);
        }
    }
}
