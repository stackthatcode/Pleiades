using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Model
{
    public class ChangeUserPasswordContext : OwnerAuthorizationContextBase
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
