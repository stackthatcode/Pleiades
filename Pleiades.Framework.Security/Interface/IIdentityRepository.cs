using Pleiades.Data;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IIdentityRepository : IGenericRepository<IdentityUser>
    {
        int GetUserCountByRole(UserRole role);
        IdentityUser RetrieveUserById(int id);
    }
}