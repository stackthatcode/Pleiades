using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contract for Persistence Layer operations needed by the PfMembership
    /// </summary>
    public interface IMembershipWritableRepository
    {
        PfMembershipUser GetUserByUserName(string username);
        PfMembershipUser GetUserByEmail(string email);
        void AddUser(PfMembershipUser user);
        void DeleteUser(string username, bool deleteAllRelatedData);
    }
}
