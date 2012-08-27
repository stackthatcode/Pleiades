using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution.Steps;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Composites
{
    public class SystemAuthorizationComposite : StepComposite<SystemAuthorizationContextBase>        
    {
        public SystemAuthorizationComposite(IGenericContainer container)
            : base(container)
        {
            // Load an Aggregate User entity based on the credentials in the HttpContext
            this.Inject<GetUserFromContextStep>();

            // Identity Authroziation
            this.Inject<AccountStatusAuthorizationStep<SystemAuthorizationContextBase>>();
            this.Inject<RoleAuthorizationStep<SystemAuthorizationContextBase>>();
            this.Inject<AccountLevelAuthorizationStep<SystemAuthorizationContextBase>>();
        }
    }
}
