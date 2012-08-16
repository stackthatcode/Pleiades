using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.Web.Security.Execution.Steps;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Execution.Composites
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
