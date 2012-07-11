﻿using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Security;
using Pleiades.Commerce.Domain.Model.Users;

namespace Pleiades.Commerce.Web.Security.Model
{
    public class OwnerAuthorizationContext : IOwnerAuthorizationContext
    {
        public AggregateUser ThisUser { get; set; }
        public AggregateUser OwnerUser { get; set; }

        public IdentityUser CurrentUserIdentity { get { return ThisUser == null ? null : ThisUser.IdentityUser; } }
        public int? OwnerIdentityId { get { return OwnerUser == null ? null : (int?)OwnerUser.IdentityUser.ID; } }

        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool ExecutionStateValid { get; set; }

        public OwnerAuthorizationContext()
        {
            this.ThisUser = null;
            this.OwnerUser = null;

            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.ExecutionStateValid = true;
        }
    }
}
