using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security.Execution.Authorization
{
    public class AuthorizeFromHttpContextStepComposite : StepComposite<AggrUserAuthContext>        
    {
        public AuthorizeFromHttpContextStepComposite(IContainer container)
            : base(container)
        {
            // Load an Aggregate User entity based on the credentials in the HttpContext
            this.Inject<GetUserFromHttpContextStep>();

            // Identity Authroziation
            this.Inject<AccountStatusAuthorizationStep<AggrUserAuthContext>>();
            this.Inject<RoleAuthorizationStep<AggrUserAuthContext>>();
            this.Inject<SimpleOwnerAuthorizationStep<AggrUserAuthContext>>();
            this.Inject<AccountLevelAuthorizationStep<AggrUserAuthContext>>();
        }
    }
}
