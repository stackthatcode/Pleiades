using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Pleiades.Commerce.Domain.Interface;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Framework.Data;
using Pleiades.Framework.Data.EF;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Persist.Users
{
    public class AggregateUserRepository : EFGenericRepository<AggregateUser>, IAggregateUserRepository
    {
        public AggregateUserRepository(DbContext context) : base(context)
        {
        }

        public AggregateUser RetrieveUserByMembershipUserName(string membershipUsername)
        {
            return this.Data().FirstOrDefault(x => x.MembershipUserName == membershipUsername);
        }

        public IEnumerable<AggregateUser> RetreiveAll(List<UserRole> roles)
        {
            return this.Data().Where(x => roles.Contains(x.IdentityUser.UserRole));
        }

        public IEnumerable<AggregateUser> 
                RetreiveByMembershipEmailAndRole(
                    List<string> membershipUserNames, List<UserRole> roles)
        {
            return this.Data().Where(x => 
                roles.Contains(x.IdentityUser.UserRole) && membershipUserNames.Contains(x.MembershipUserName));
        }
    }
}
