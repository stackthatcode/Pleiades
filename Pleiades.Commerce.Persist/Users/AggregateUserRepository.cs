using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Domain.Interface;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Persist.Security
{
    public class AggregateUserRepository : EFGenericRepository<AggregateUser>, IAggregateUserRepository
    {
        public AggregateUserRepository(DbContext context) : base(context)
        {
        }

        // Exists to patch the Membership User's isolated Database Context issues
        public MembershipUser RetreiveMembershipUser(string membershipUsername)
        {
            return this.Context
                .Set<MembershipUser>()
                .FirstOrDefault(x => x.UserName == membershipUsername);
        }

        public AggregateUser RetrieveByMembershipUserName(string membershipUsername)
        {
            this.Context.Set<MembershipUser>();

            var output = this.TrackableData()
                .Include(x => x.IdentityUser)
                .FirstOrDefault(x => x.MembershipUser.UserName == membershipUsername);
            return output;   
        }

        public IEnumerable<AggregateUser> Retreive(List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());
            return this.TrackableData().Where(x => enumRoles.Contains(x.IdentityUser.UserRoleValue));
        }

        public IEnumerable<AggregateUser> Retreive(List<string> membershipUserNames, List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());
            return this.TrackableData().Where(x =>
                enumRoles.Contains(x.IdentityUser.UserRoleValue) && 
                membershipUserNames.Contains(x.MembershipUser.UserName));
        }
    }
}