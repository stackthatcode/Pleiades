using System.Collections.Generic;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Interface
{
    public interface IAggregateUserRepository : IGenericRepository<AggregateUser>
    {
        AggregateUser RetrieveByMembershipUserName(string username);
        IEnumerable<AggregateUser> Retreive(List<UserRole> role);
        IEnumerable<AggregateUser> Retreive(List<string> membershipUserNames, List<UserRole> role);
    }
}