using System.Collections.Generic;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IReadOnlyAggregateUserRepository
    {
        AggregateUser RetrieveByMembershipUserName(string username);
        AggregateUser RetrieveById(int aggregateUserId);
        IEnumerable<AggregateUser> Retreive(List<UserRole> role);
        int GetUserCountByRole(UserRole role);
    }
}