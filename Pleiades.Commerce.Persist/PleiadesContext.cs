using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Commerce.Domain.Model.Products;


namespace Pleiades.Commerce.Persist
{
    public class PleiadesContext : DbContext
    {
        public DbSet<AggregateUser> AggregateUsers { get; set; }
        public DbSet<MembershipUser> MembershipUsers { get; set; }
        public DbSet<IdentityUser> IdentityUsers { get; set; }
        
        // public DbSet<AggregateUser> AggregateUsers { get; set; } -- todo: add Category

        public PleiadesContext() : base("PleiadesDb")
        {
        }

        public PleiadesContext(string conn) : base(conn)
        {
        }
    }
}
