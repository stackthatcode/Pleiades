using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security.Execution
{
    public class AuthorizationStepComposite : StepComposite<CommerceSecurityContext>        
    {
        public AuthorizationStepComposite(IContainer container) : base(container)
        {
            this.Inject<GetUserFromHttpContextStep>();

            this.Inject<AccountStatusAuthorizationStep<CommerceSecurityContext>>();
            this.Inject<RoleAuthorizationStep<CommerceSecurityContext>>();
            this.Inject<SimpleOwnerAuthorizationStep<CommerceSecurityContext>>();
            this.Inject<AccountLevelAuthorizationStep<CommerceSecurityContext>>();
        }
    }
}
