using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Context
{
    public class ChangeUserPasswordContext : OwnerAuthorizationContext
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
