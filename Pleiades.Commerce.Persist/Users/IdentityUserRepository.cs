using System.Linq;
using System.Data.Entity;
using Pleiades.Framework.Data;
using Pleiades.Framework.Data.EF;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Commerce.Persist.Users
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
