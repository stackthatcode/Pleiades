using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Identity.Interface
{
    public interface IAggregateUserRepository
    {
        // Move these into the "Combined User Service"
        DomainUser RetrieveUserByMembershipUserName(string username);
        DomainUser RetrieveUserByEmail(string emailaddr);
        IEnumerable<DomainUserCondensed> RetreiveAll(int pageNumber, int pageSize, List<UserRole> role);
        IEnumerable<DomainUserCondensed> RetreiveByLikeEmail(
            string emailAddressToMatch, int pageNumber, int pageSize, List<UserRole> role);
    }
}
