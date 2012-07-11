using System.Collections.Generic;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Domain.Interface
{
    public interface IAggregateUserRepository : IGenericRepository<AggregateUser>
    {
        AggregateUser RetrieveUserByMembershipUserName(string username);
        AggregateUser RetrieveUserByEmail(string emailaddr);
        IEnumerable<AggregateUser> RetreiveAll(List<UserRole> role);
        IEnumerable<AggregateUser> RetreiveByLikeEmail(string emailAddressToMatch, List<UserRole> role);
    }
}