using System.Collections.Generic;
using Pleiades.Data;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IAggregateUserRepository : IGenericRepository<AggregateUser>
    {
        AggregateUser RetrieveByMembershipUserName(string username);
        AggregateUser RetrieveById(int Id);
        MembershipUser RetreiveMembershipUser(string username);
        IEnumerable<AggregateUser> Retreive(List<UserRole> role);
        IEnumerable<AggregateUser> Retreive(List<string> membershipUserNames, List<UserRole> role);
        int GetUserCountByRole(UserRole role);
        void UpdateIdentity(CreateOrModifyIdentityRequest changes);
        void Delete(int aggregateUserID);
    }
}