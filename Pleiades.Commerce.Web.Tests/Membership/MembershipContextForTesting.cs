using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Web.IntegrationTests.Membership
{
    public class MembershipContextForTesting : DbContext
    {
        public DbSet<MembershipUser> MyEntities { get; set; }

        public MembershipContextForTesting() : base("MyTestDatabase")
        {
        }

        public MembershipContextForTesting(string conn) : base(conn)
        {
        }
    }
}