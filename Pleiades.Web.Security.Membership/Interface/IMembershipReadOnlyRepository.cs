using System;
using System.Collections.Generic;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IMembershipReadOnlyRepository
    {
        PfMembershipUser GetUserByUserName(string username);
        PfMembershipUser GetUserByEmail(string email);
        int GetNumberOfUsersOnline(TimeSpan userIsOnlineTimeWindow);
        IEnumerable<PfMembershipUser> GetAllUsers();
    }
}
