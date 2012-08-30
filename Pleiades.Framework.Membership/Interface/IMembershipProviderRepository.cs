using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Data;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contract for Persistence Layer operations needed by the Membership Provider
    /// </summary>
    public interface IMembershipProviderRepository : IGenericRepository<MembershipUser>
    {
        string ApplicationName { get; set; }

        MembershipUser GetUser(string username);
        MembershipUser GetUserByProviderKey(object providerKey);
        string GetUserNameByEmail(string email);
        bool DeleteUser(string username, bool deleteAllRelatedData);

        int GetNumberOfUsersOnline(TimeSpan userIsOnlineTimeWindow);
        IList<MembershipUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);
        IList<MembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);
        IList<MembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);
    }
}