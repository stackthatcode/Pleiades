using System.Collections.Generic;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Execution.Steps;

namespace Pleiades.Web.Security.Execution.Abstract
{
    public abstract class OwnerAuthCompositeBase<T> : StepComposite<T>
            where T : OwnerAuthorizationContextBase
    {
        public OwnerAuthCompositeBase(IGenericContainer container, Step<T> guardedSteps)
            : base(container)
        {
            // Authorization Step
            this.Inject<SimpleOwnerAuthorizationStep<T>>();

            // Guarded Steps
            this.Register(guardedSteps);
        }
    }
}