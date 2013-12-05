using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Database;
using Pleiades.App.Data;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Application.Security
{
    public class AggregateReadOnlyRepository : EFGenericRepository<AggregateUser>, IReadOnlyAggregateUserRepository
    {
        public AggregateReadOnlyRepository(PushMarketContext context) : base(context)
        {
        }

        protected override IQueryable<AggregateUser> ReadOnlyData()
        {
            return base.ReadOnlyData()
                       .Include(x => x.IdentityProfile)
                       .Include(x => x.Membership);
        }

        public AggregateUser RetrieveByMembershipUserName(string membershipUsername)
        {
            this.Context.Set<PfMembershipUser>();

            var output = this.ReadOnlyData()
                .FirstOrDefault(x => x.Membership.UserName == membershipUsername);
            return output;   
        }

        public IEnumerable<AggregateUser> Retreive(List<UserRole> roles)
        {
            var enumRoles = roles.Select(x => x.ToString());
            return this.ReadOnlyData()
                .Where(x => enumRoles.Contains(x.IdentityProfile.UserRoleValue));
        }

        public AggregateUser RetrieveById(int aggregateUserId)
        {
            return this.ReadOnlyData().FirstOrDefault(x => x.ID == aggregateUserId);
        }

        public int GetUserCountByRole(UserRole role)
        {
            var roleName = role.ToString();
            return this.ReadOnlyData().Count(x => x.IdentityProfile.UserRoleValue == roleName);
        }
    }
}
