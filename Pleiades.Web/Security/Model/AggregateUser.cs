using System;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Model
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
                    MembershipUser = new MembershipUser
                    {
                    }
                };
        }
    }
}