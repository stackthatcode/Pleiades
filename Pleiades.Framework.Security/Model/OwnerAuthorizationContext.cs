using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Identity.Model
{
    public class OwnerAuthorizationContext : IOwnerAuthorizationContext
    {
        public IdentityUser CurrentUserIdentity { get; set; }
        public int? OwnerIdentityId { get; set; }

        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool ExecutionStateValid { get; set; }


        public OwnerAuthorizationContext()
        {
            this.CurrentUserIdentity = null;
            this.OwnerIdentityId = null;

            this.SecurityResponseCode = Security.SecurityResponseCode.Allowed;
            this.ExecutionStateValid = true;
        }       
    }
}