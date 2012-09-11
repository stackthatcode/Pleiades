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

        void SetUserPassword(int id, string password);
        string ResetPassword(int id);
        void SetUserApproval(int id, bool isApproved);
        void ChangeEmailAddress(int id, string email);
        void UnlockUser(int id);
    }
}