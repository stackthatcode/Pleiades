using System;
using System.Collections.Generic;
using Pleiades.Application.Data;
using Pleiades.Application;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contract for Persistence Layer operations needed by the Membership Provider
    /// </summary>
    public interface IMembershipProviderRepository : IGenericRepository<PfMembershipUser>
    {
        string ApplicationName { get; set; }

        PfMembershipUser GetUser(string username);
        PfMembershipUser GetUserByProviderKey(object providerKey);
        string GetUserNameByEmail(string email);
        
        int GetNumberOfUsersOnline(TimeSpan userIsOnlineTimeWindow);
        IList<PfMembershipUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);

        IList<PfMembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);
        IList<PfMembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);
        IList<PfMembershipUser> FindUsersByEmail(string emailToMatch);

        bool DeleteUser(string username, bool deleteAllRelatedData);
    }
}
