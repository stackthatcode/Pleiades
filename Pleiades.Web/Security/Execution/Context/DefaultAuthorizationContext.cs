using Pleiades.Security;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Execution.Context
{
    public class DefaultAuthorizationContext : ISystemAuthorizationContext
    {
        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool IsExecutionStateValid { get; set; }
    }
}
