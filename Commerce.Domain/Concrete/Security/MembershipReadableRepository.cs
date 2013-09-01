using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Database;
using Pleiades.Application.Data.EF;
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

        PfMembershipUser IMembershipReadOnlyRepository.GetUserByUserName(string username)
        {
            return this.ReadOnlyData().FirstOrDefault(x => x.UserName == username);
        }

        PfMembershipUser IMembershipReadOnlyRepository.GetUserByEmail(string email)
        {
            return this.ReadOnlyData().FirstOrDefault(x => x.Email == email);
        }

        // TODO: build this out...?
        public int GetNumberOfUsersOnline(TimeSpan userIsOnlineTimeWindow)
        {
            //e DateTime.Now - userIsOnlineTimeWindow
            throw new System.NotImplementedException();
        }

        public IEnumerable<PfMembershipUser> GetAllUsers()
        {
            return this.ReadOnlyData().Where(x => true);
        }
    }
}
