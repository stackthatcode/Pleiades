using System.Collections.Generic;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;

namespace Pleiades.Web.Security.Execution.Composites
{
    public abstract class OwnerSecuredCompositeBase<T> : StepComposite<T> where T : OwnerAuthorizationContext
    {
        public OwnerSecuredCompositeBase(IGenericContainer container, Step<T> guardedSteps)
            : base(container)
        {
            // Authorization Step
            this.Inject<SimpleOwnerAuthorizationStep<T>>();

            // Guarded Steps
            this.Register(guardedSteps);
        }
    }
}