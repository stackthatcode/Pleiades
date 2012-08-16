using System;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Framework.Web.Security.Model
{
    public class AggregateUser
    {
        public int ID { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public string MembershipUserName { get; set; }

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
