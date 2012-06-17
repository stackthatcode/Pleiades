using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Framework.Data;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Framework.MembershipProvider.Interface
{
    /// <summary>
    /// Contract for Persistence Layer operations needed by the Membership Provider
    /// </summary>
    public interface IMembershipRepository : IGenericRepository<MembershipUser>
    {
        string ApplicationName { get; set; }
        MembershipUser GetUser(string username);
        MembershipUser GetUser(string username, bool userIsOnline);
        MembershipUser GetUserByProviderKey(object providerKey, bool userIsOnline);
        string GetUserNameByEmail(string email);
        bool DeleteUser(string username, bool deleteAllRelatedData);
        IList<MembershipUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);
        int GetNumberOfUsersOnline();
        IList<MembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);
        IList<MembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);
    }
}