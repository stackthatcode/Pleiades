using System;
using System.Web;
using System.Collections.Generic;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    //
    // The pass-through functions to MembershipService dependency exist to prevent bad, bad things like
    // reseting the Supreme User's password.  Initializer-type applications can use lower-level stuff like the 
    // MembershipService to perform these functions.
    //
    // UPDATE: this commentary is still relevant.  This is a classic Entity Root Aggregate
    //
    public interface IAggregateUserService
    {
        Guid Tracer { get; }

        AggregateUser Create(
            PfCreateNewMembershipUserRequest membershipUser, 
            CreateOrModifyIdentityRequest identityUser,
            out PleiadesMembershipCreateStatus outStatus);

        bool Authenticate(string username, string password, bool persistenceCookie, List<UserRole> expectedRoles);
        AggregateUser LoadAuthentedUserIntoContext(HttpContextBase context);
        void UpdateIdentity(CreateOrModifyIdentityRequest identityUserRequest);
        // void UpdateEmailAndApproval(int aggregateUserId, string email, bool approval);
        void ChangeUserPassword(int aggregateUserId, string oldPassword, string newPassword);
    }
}