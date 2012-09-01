using Pleiades.Data;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IIdentityRepository : IGenericRepository<IdentityUser>
    {
        IdentityUser RetrieveUserById(int id);
    }
}