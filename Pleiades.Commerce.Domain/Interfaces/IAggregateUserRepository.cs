using System.Collections.Generic;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Domain.Interface
{
    public interface IAggregateUserRepository : IGenericRepository<AggregateUser>
    {
        AggregateUser RetrieveByMembershipUserName(string username);
        IEnumerable<AggregateUser> Retreive(List<UserRole> role);
        IEnumerable<AggregateUser> Retreive(List<string> membershipUserNames, List<UserRole> role);
    }
}