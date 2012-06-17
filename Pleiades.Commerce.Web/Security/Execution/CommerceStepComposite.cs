using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security.Execution
{
    public class CommerceStepComposite : StepComposite<CommerceSecurityContext>        
    {
        public CommerceStepComposite(Container container) : base(container)
        {
            this.Inject<GetUserFromHttpContextStep>();
            this.Inject<CompositeIdentityAuthStep<CommerceSecurityContext>>();
        }
    }
}
