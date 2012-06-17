using System.Data.Entity;
using Pleiades.Framework.Data;
using Pleiades.Framework.Data.EF;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Commerce.Persist
{
    public class IdentityUserRepository : EFGenericRepository<IdentityUser>, IIdentityRepository
    {
        public IdentityUserRepository(DbContext context)
            : base(context)
        {
        }
    }
}
