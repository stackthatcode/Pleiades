using System;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Domain.Entities.Users
{
    public class AggregateUser
    {
        public int ID { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public MembershipUser MembershipUser { get; set; }

        public static AggregateUser AnonymousUserFactory()
        {
            return 
                new AggregateUser
                {
                     IdentityUser = new IdentityUser
                     {
                         UserRole = UserRole.Anonymous
                     },
                };
        }
    }
}
