using System;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Model
{
    public class AggregateUser
    {
        public int ID { get; set; }
        public IdentityUser Identity { get; set; }
        public MembershipUser Membership { get; set; }

        public static AggregateUser AnonymousUserFactory()
        {
            return
                new AggregateUser
                {
                    Identity = new IdentityUser
                    {
                        UserRole = UserRole.Anonymous
                    },
                    Membership = new MembershipUser
                    {
                    }
                };
        }
    }
}