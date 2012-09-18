using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Composites
{
    public class SystemAuthorizationComposite : CompositeStep<SystemAuthorizationContext>
    {
        public SystemAuthorizationComposite(IServiceLocator container)
            : base(container)
        {
            this.ResolveAndAdd<GetUserFromHttpContextStep>();
            this.ResolveAndAdd<AccountStatusAuthorizationStep>();
            this.ResolveAndAdd<RoleAuthorizationStep>();
            this.ResolveAndAdd<AccountLevelAuthorizationStep>();
        }
    }
}