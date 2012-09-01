using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Composites
{
    public class SystemAuthorizationComposite : StepComposite<SystemAuthorizationContext>        
    {
        public SystemAuthorizationComposite(IGenericContainer container)
            : base(container)
        {
            this.Inject<GetUserFromContextStep>();
            this.Inject<AccountStatusAuthorizationStep>();
            this.Inject<RoleAuthorizationStep>();
            this.Inject<AccountLevelAuthorizationStep>();
        }
    }
}
