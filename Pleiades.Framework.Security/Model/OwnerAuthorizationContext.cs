using Pleiades.Security;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Model
{
    public class OwnerAuthorizationContext : IOwnerAuthorizationContext
    {
        public IdentityUser CurrentUserIdentity { get; set; }
        public int? OwnerIdentityId { get; set; }

        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool IsExecutionStateValid { get; set; }


        public OwnerAuthorizationContext()
        {
            this.CurrentUserIdentity = null;
            this.OwnerIdentityId = null;

            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.IsExecutionStateValid = true;
        }       
    }
}