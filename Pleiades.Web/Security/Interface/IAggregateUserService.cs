using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IAggregateUserService
    {
        AggregateUser Create(
            CreateNewMembershipUserRequest membershipUser, 
            CreateOrModifyIdentityUserRequest identityUser,
            out PleiadesMembershipCreateStatus outStatus);
    }
}
