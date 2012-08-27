using System;
using Pleiades.Security;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Model
{
    public class OwnerAuthorizationContextBase : IOwnerAuthorizationContext
    {
        public AggregateUser ThisUser { get; set; }
        public AggregateUser OwnerUser { get; set; }

        public IdentityUser CurrentUserIdentity { get { return ThisUser == null ? null : ThisUser.IdentityUser; } }
        public int? OwnerIdentityId { get { return OwnerUser == null ? null : (int?)OwnerUser.IdentityUser.ID; } }

        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool IsExecutionStateValid { get; set; }

        public OwnerAuthorizationContextBase()
        {
            this.ThisUser = null;
            this.OwnerUser = null;

            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.IsExecutionStateValid = true;
        }
    }
}
