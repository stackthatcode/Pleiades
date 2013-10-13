using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Commerce.Application.Model.Shopping;
using Pleiades.Web.Security.Model;
using Commerce.Application.Model.Billing;
using Commerce.Application.Model.Lists;
using Commerce.Application.Model.Orders;
using Commerce.Application.Model.Products;
using Commerce.Application.Model.Resources;

namespace Commerce.Application.Database
{
    public class PushMarketContext : DbContext
    {
        public readonly Guid ContextId = Guid.NewGuid();
        
        // Users
        public IDbSet<AggregateUser> AggregateUsers { get; set; }
        public IDbSet<PfMembershipUser> MembershipUsers { get; set; }
        public IDbSet<IdentityProfile> IdentityProfiles { get; set; }

        // Lists
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<CategoryHierarchy> CategoryHierarchies { get; set; } 
        public IDbSet<SizeGroup> SizeGroups { get; set; }
        public IDbSet<Size> Sizes { get; set; }
        public IDbSet<Brand> Brands { get; set; }
        public IDbSet<Color> Colors { get; set; }

        // Resources
        public IDbSet<FileResource> FileResources { get; set; }
        public DbSet<ImageBundle> ImageBundles { get; set; }
        
        // Products
        public IDbSet<Product> Products { get; set; }
        public IDbSet<ProductImage> ProductImages { get; set; }
        public IDbSet<ProductColor> ProductColors { get; set; }
        public IDbSet<ProductSku> ProductSkus { get; set; }

        // Orders
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderLine> OrderLines { get; set; }
        public IDbSet<ShippingMethod> ShippingMethods { get; set; }

        // Billing
        public IDbSet<StateTax> StateTaxes { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }
        public IDbSet<Total> Totals { get; set; }

        // Carts
        public IDbSet<Cart> Carts { get; set; }
        public IDbSet<CartItem> CartItems { get; set; }

        public PushMarketContext() : base("PleiadesDb")
        {
            System.Data.Entity.Database.SetInitializer<PushMarketContext>(null);
            Configuration.AutoDetectChangesEnabled = true;
        }

        public void MarkModified(object target)
        {
            this.Entry(target).State = EntityState.Modified;
        }

        public void MasterDatabaseCreate()
        {
            if (!Database.Exists())
            {
                Database.Create();
                AddViews();
            }
        }

        private void AddViews()
        {
            Database.ExecuteSqlCommand("DROP TABLE [dbo].[CategoryHierarchies]");
            Database.ExecuteSqlCommand(
                @"CREATE VIEW [dbo].[CategoryHierarchies] AS
                WITH CTE ( ParentId, Id, Name ) AS 
                ( 
	                SELECT   Id, Id, Name 
	                FROM     dbo.Categories 
	
	                UNION ALL 
   
	                SELECT   categories.Id, CTE.Id, categories.Name
	                FROM     CTE INNER JOIN dbo.Categories AS categories ON categories.ParentId = cte.ParentId 
                )
                SELECT  ISNULL(Id, 0) AS ParentId , ISNULL(ParentId, 0) AS Id, Name
                FROM    CTE");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>()
                .HasOptional(x => x.ImageBundle)
                .WithMany()
                .HasForeignKey(x => x.ImageBundleId);

            modelBuilder.Entity<Color>()
                .HasOptional(x => x.ImageBundle)
                .WithMany()
                .HasForeignKey(x => x.ImageBundleId);
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
