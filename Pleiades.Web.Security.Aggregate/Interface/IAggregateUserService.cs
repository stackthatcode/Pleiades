using System.Web;
using System.Collections.Generic;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IAggregateUserService
    {
        AggregateUser Create(
            PfCreateNewMembershipUserRequest membershipUser, IdentityProfileChange identityUser, out string outStatus);
        AggregateUser Authenticate(string username, string password, bool persistenceCookie, List<UserRole> expectedRoles);
        AggregateUser LoadAuthentedUserIntoContext(HttpContextBase context);
        void UpdateIdentity(int id, IdentityProfileChange identityChange);
        void Delete(int id);
    }
}
