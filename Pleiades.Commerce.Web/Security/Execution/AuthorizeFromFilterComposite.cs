using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security.Execution
{
    public class AuthorizeFromFilterComposite : StepComposite<SystemAuthorizationContextBase>        
    {
        public AuthorizeFromFilterComposite(IContainer container)
            : base(container)
        {
            // Load an Aggregate User entity based on the credentials in the HttpContext
            this.Inject<GetUserFromFilterContextStep>();

            // Identity Authroziation
            this.Inject<AccountStatusAuthorizationStep<SystemAuthorizationContextBase>>();
            this.Inject<RoleAuthorizationStep<SystemAuthorizationContextBase>>();
            this.Inject<AccountLevelAuthorizationStep<SystemAuthorizationContextBase>>();
        }
    }
}
