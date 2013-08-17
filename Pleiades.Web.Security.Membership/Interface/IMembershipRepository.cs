using System;
using System.Collections.Generic;
using Pleiades.Data;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contract for Persistence Layer operations needed by the PfMembership
    /// </summary>
    public interface IMembershipRepository : IGenericRepository<PfMembershipUser>
    {
        string ApplicationName { get; set; }    // Multi tenancy??? Not in this context, dude.

        PfMembershipUser GetUserByUserName(string username);
        PfMembershipUser GetUserByEmail(string email);
        int GetNumberOfUsersOnline(TimeSpan userIsOnlineTimeWindow);

        IEnumerable<PfMembershipUser> GetAllUsers();
        IEnumerable<PfMembershipUser> FindUsersByName(string usernameToMatch);
        IEnumerable<PfMembershipUser> FindUsersByEmail(string emailToMatch);

        bool DeleteUser(string username, bool deleteAllRelatedData);
    }
}
