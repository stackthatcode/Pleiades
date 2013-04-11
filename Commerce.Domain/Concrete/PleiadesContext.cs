using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Pleiades.Web.Security.Model;
using Commerce.Persist.Model.Billing;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Orders;
using Commerce.Persist.Model.Products;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Concrete
{
    public class PleiadesContext : DbContext
    {
        public readonly Guid ContextId = Guid.NewGuid();
        
        // Users
        public DbSet<AggregateUser> AggregateUsers { get; set; }
        public DbSet<PfMembershipUser> MembershipUsers { get; set; }
        public DbSet<IdentityProfile> IdentityProfiles { get; set; }

        // Lists
        public DbSet<Category> Categories { get; set; }
        public DbSet<SizeGroup> SizeGroups { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }

        // Resources
        public DbSet<FileResource> FileResources { get; set; }
        public DbSet<ImageBundle> ImageBundles { get; set; }
        
        // Products
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSku> ProductSkus { get; set; }

        // Orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }

        // Billing
        public DbSet<StateTax> StateTaxes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Experimental
        public DbSet<SmallIdea> SmallIdeas { get; set; }


        public PleiadesContext() : base("PleiadesDb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public void RefreshCollection<T>(IEnumerable<T> collection)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.Refresh(System.Data.Objects.RefreshMode.StoreWins, collection);
        }

        public void RefreshEntity<T>(T entity)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.Refresh(System.Data.Objects.RefreshMode.StoreWins, entity);
        }
    }
}
