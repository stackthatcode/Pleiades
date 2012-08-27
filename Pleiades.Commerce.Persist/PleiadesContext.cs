using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Pleiades.Web.Security.Model;

namespace Commerce.Persist
{
    public class PleiadesContext : DbContext
    {
        public readonly Guid ContextId = Guid.NewGuid();
        
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
