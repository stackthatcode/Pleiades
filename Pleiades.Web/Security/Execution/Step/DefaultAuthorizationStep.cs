using Pleiades.Execution;
using Pleiades.Security;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Execution.Step
{
    public class DefaultAuthorizationStep : Step<ISystemAuthorizationContext>
    {
        public override ISystemAuthorizationContext Execute(ISystemAuthorizationContext context)
        {
            context.SecurityResponseCode = SecurityResponseCode.Allowed;
            return context;
        }
    }
}