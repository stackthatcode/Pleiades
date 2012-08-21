using System.Collections.Generic;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.Web.Security.Execution;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Web.Security.Execution.Steps;

namespace Pleiades.Framework.Web.Security.Execution.Abstract
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