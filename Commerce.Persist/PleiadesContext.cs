using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Pleiades.Web.Security.Model;
using Commerce.Domain.Model.Lists;

namespace Commerce.Persist
{
    public class PleiadesContext : DbContext
    {
        public readonly Guid ContextId = Guid.NewGuid();
        
        public DbSet<AggregateUser> AggregateUsers { get; set; }
        public DbSet<MembershipUser> MembershipUsers { get; set; }
        public DbSet<IdentityProfile> IdentityProfiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SizeGroup> SizeGroups { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        

        public PleiadesContext() : base("PleiadesDb")
        {
        }

        public PleiadesContext(string conn) : base(conn)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AggregateUser>().
            // {
            //     m.ToTable("UserRoles");
            //     m.MapLeftKey("UserId");
            //     m.MapRightKey("RoleId");
            // });
        }
    }
}
