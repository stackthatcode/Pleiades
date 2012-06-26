using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Framework.IntegrationTests.Membership
{
    public class MembershipContext : DbContext
    {
        public DbSet<MembershipUser> MyEntities { get; set; }

        public MembershipContext() : 
            base("MyTestDatabase")
        {
        }

        public MembershipContext(string conn)
            : base(conn)
        {
        }
    }
}
