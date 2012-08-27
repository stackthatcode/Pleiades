using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Domain.Interface;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Persist.Security
{
    public class AggregateUserRepository : EFGenericRepository<AggregateUser>, IAggregateUserRepository
    {
        public AggregateUserRepository(DbContext context) : base(context)
        {
        }

        public AggregateUser RetrieveByMembershipUserName(string membershipUsername)
        {
            return this.Data()
                .Include(x => x.IdentityUser)
                .FirstOrDefault(x => x.MembershipUserName == membershipUsername);
        }

        public IEnumerable<AggregateUser> Retreive(List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());
            return this.Data().Where(x => enumRoles.Contains(x.IdentityUser.UserRoleValue));
        }

        public IEnumerable<AggregateUser> Retreive(List<string> membershipUserNames, List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());
            return this.Data().Where(x =>
                enumRoles.Contains(x.IdentityUser.UserRoleValue) && membershipUserNames.Contains(x.MembershipUserName));
        }
    }
}