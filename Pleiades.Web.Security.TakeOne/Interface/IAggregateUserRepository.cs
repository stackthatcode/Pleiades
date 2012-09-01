using System.Collections.Generic;
using Pleiades.Data;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IAggregateUserRepository : IGenericRepository<AggregateUser>
    {
        MembershipUser RetreiveMembershipUser(string username);
        AggregateUser RetrieveByMembershipUserName(string username);
        int GetUserCountByRole(UserRole role);
        IEnumerable<AggregateUser> Retreive(List<UserRole> role);
        IEnumerable<AggregateUser> Retreive(List<string> membershipUserNames, List<UserRole> role);
    }
}