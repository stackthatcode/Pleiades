using System;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Model
{
    public class AggregateUser
    {
        public int ID { get; set; }
        public IdentityProfile IdentityProfile { get; set; }
        public MembershipUser Membership { get; set; }

        public static AggregateUser AnonymousFactory()
        {
            return
                new AggregateUser
                {
                    IdentityProfile = new IdentityProfile
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