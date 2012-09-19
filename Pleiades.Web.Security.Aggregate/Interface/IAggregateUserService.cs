using System.Collections.Generic;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    // The pass-through functions to MembershipService dependency exist to prevent bad, bad things like
    // reseting the Supreme User's password.  Initializer-type applications can use lower-level stuff like the 
    // MembershipService to perform these functions.

    public interface IAggregateUserService
    {
        AggregateUser Create(
            CreateNewMembershipUserRequest membershipUser, 
            CreateOrModifyIdentityRequest identityUser,
            out PleiadesMembershipCreateStatus outStatus);

        bool Authenticate(string username, string password, bool persistenceCookie, List<UserRole> expectedRoles);

        void UpdateIdentity(int aggregateUserId, CreateOrModifyIdentityRequest identityUserRequest);

        void UpdateEmail(int aggregateUserId, string email);

        void UpdateApproval(int aggregateUserId, bool approval);

        void ChangeUserPassword(int aggregateUserId, string oldPassword, string newPassword);
    }
}