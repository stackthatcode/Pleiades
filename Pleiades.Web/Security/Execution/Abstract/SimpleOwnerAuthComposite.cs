using System.Collections.Generic;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.Web.Security.Execution;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Web.Security.Execution.Steps;

namespace Pleiades.Framework.Web.Security.Execution.Abstract
{
    // TODO: possibly move this into Identity Framework?

    public abstract class SimpleOwnerAuthComposite<T> : StepComposite<T>
            where T : OwnerAuthorizationContextBase
    {
        public SimpleOwnerAuthComposite(IGenericContainer container, Step<T> guardedSteps)
            : base(container)
        {
            // Authorization Step
            this.Inject<SimpleOwnerAuthorizationStep<T>>();

            // Guarded Steps
            this.Register(guardedSteps);
        }
    }
}
