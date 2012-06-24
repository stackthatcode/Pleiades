using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Interface
{
    public interface IIdentityRepository : IGenericRepository<IdentityUser>
    {
        int GetUserCountByRole(UserRole role);
        IdentityUser RetrieveUserById(int id);
    }
}