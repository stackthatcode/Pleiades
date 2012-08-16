using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Interface
{
    public interface IAggregateUserService
    {
        AggregateUser Create(
            CreateNewMembershipUserRequest membershipUser, 
            CreateOrModifyIdentityUserRequest identityUser,
            out PleiadesMembershipCreateStatus outStatus);
    }
}
