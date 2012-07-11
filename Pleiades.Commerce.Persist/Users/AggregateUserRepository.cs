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

namespace Pleiades.Commerce.Persist
{
    public class AggregateUserRepository : EFGenericRepository<AggregateUser>, IAggregateUserRepository
    {
        public AggregateUserRepository(DbContext context) : base(context)
        {
        }

        public AggregateUser RetrieveUserByMembershipUserName(string username)
        {
            return this.Data().FirstOrDefault(x => x.MembershipUser.UserName == username);
        }

        public AggregateUser RetrieveUserByEmail(string emailaddr)
        {
            return this.Data().FirstOrDefault(x => x.MembershipUser.Email == emailaddr);
        }

        public IEnumerable<AggregateUser> RetreiveAll(List<UserRole> roles)
        {
            return this.Data().Where(x => roles.Contains(x.IdentityUser.UserRole));
        }

        public IEnumerable<AggregateUser> RetreiveByLikeEmail(string emailAddressToMatch, List<UserRole> roles)
        {
            return this.Data().Where(x => 
                    roles.Contains(x.IdentityUser.UserRole) && 
                    x.MembershipUser.Email.Contains(emailAddressToMatch));
        }
    }
}
