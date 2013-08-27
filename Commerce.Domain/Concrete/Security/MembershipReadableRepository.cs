using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Database;
using Pleiades.Application.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Application.Concrete.Security
{
    public class MembershipReadableRepository : EFGenericRepository<PfMembershipUser>, IMembershipReadOnlyRepository
    {
        public MembershipReadableRepository(PushMarketContext context)
            : base(context)
        {
        }

        protected override IQueryable<PfMembershipUser> Data()
        {
            return base.ReadOnlyData();
        }

        PfMembershipUser IMembershipReadOnlyRepository.GetUserByUserName(string username)
        {
            return this.FirstOrDefault(x => x.UserName == username);
        }

        PfMembershipUser IMembershipReadOnlyRepository.GetUserByEmail(string email)
        {
            return this.FirstOrDefault(x => x.Email == email);
        }

        public int GetNumberOfUsersOnline(TimeSpan userIsOnlineTimeWindow)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PfMembershipUser> GetAllUsers()
        {
            return this.Where(x => true);
        }
    }
}
