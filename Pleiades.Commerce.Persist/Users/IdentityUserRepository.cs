using System.Linq;
using System.Data.Entity;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Persist.Security
{
    public class IdentityUserRepository : EFGenericRepository<IdentityUser>, IIdentityRepository
    {
        public IdentityUserRepository(DbContext context)
            : base(context)
        {
        }

        public int GetUserCountByRole(UserRole role)
        {
            var enumRole = role.ToString();
            return this.Data().Count(x => x.UserRoleValue == enumRole);
        }

        public IdentityUser RetrieveUserById(int id)
        {
            return this.FindFirstOrDefault(x => x.ID == id);
        }
    }
}
