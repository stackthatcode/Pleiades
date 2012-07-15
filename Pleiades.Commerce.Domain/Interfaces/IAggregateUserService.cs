﻿using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Domain.Interface
{
    public interface IAggregateUserService
    {
        AggregateUser Create(
            CreateNewMembershipUserRequest membershipUser, 
            CreateOrModifyIdentityUserRequest identityUser,
            out PleiadesMembershipCreateStatus outStatus);
    }
}
