using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Commerce.Application.Analytics.Entities;
using Commerce.Application.Billing;
using Commerce.Application.File.Entities;
using Commerce.Application.Lists.Entities;
using Commerce.Application.Orders.Entities;
using Commerce.Application.Products;
using Commerce.Application.Products.Entities;
using Commerce.Application.Shopping.Entities;
using Pleiades.Web.Security.Model;


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

        // Billing
        public IDbSet<StateTax> StateTaxes { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }
        public IDbSet<Total> Totals { get; set; }

        // Carts
        public IDbSet<Cart> Carts { get; set; }
        public IDbSet<CartItem> CartItems { get; set; }

        // Orders
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderLine> OrderLines { get; set; }
        public IDbSet<ShippingMethod> ShippingMethods { get; set; }

        // Analytics
        public IDbSet<PurchaseOrderEvent> PurchaseOrderEvents { get; set; }
        public IDbSet<PurchaseSkuEvent> PurchaseSkuEvents { get; set; }
        public IDbSet<RefundEvent> RefundEvents { get; set; }
        public IDbSet<RefundSkuEvent> RefundSkuEvents { get; set; }


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

            var orderLine = modelBuilder.Entity<OrderLine>();
            orderLine.HasKey(x => x.Id);
            orderLine.Property(x => x.OriginalName);
            orderLine.Property(x => x.OriginalSkuCode);
            orderLine.Property(x => x.OriginalUnitPrice);
            orderLine.Property(x => x.Quantity);
            orderLine.Property(x => x.OrderLineStatusValue);
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
