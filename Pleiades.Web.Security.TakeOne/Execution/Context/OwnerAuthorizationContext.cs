using System;
using Pleiades.Execution;
using Pleiades.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Context
{
    public class OwnerAuthorizationContext : IStepContext
    {
        public AggregateUser ThisUser { get; set; }
        public AggregateUser OwnerUser { get; set; }

        public IdentityProfile CurrentUserIdentity { get { return ThisUser == null ? null : ThisUser.IdentityProfile; } }
        public int? OwnerIdentityId { get { return OwnerUser == null ? null : (int?)OwnerUser.IdentityProfile.ID; } }

        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool IsExecutionStateValid { get; set; }

        public OwnerAuthorizationContext()
        {
            this.ThisUser = null;
            this.OwnerUser = null;

            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.IsExecutionStateValid = true;
        }
    }
}