using System.Configuration;
using System.Data.Entity;
using System.Web;
using Autofac;
using Pleiades.App.Data;
using Pleiades.App.Utility;
using Pleiades.Web.Security.Interface;
using Commerce.Application.Analytics;
using Commerce.Application.Azure;
using Commerce.Application.Email;
using Commerce.Application.File;
using Commerce.Application.Lists;
using Commerce.Application.Lists.Entities;
using Commerce.Application.Orders;
using Commerce.Application.Payment;
using Commerce.Application.Products;
using Commerce.Application.Products.Entities;
using Commerce.Application.Security;
using Commerce.Application.Shopping;
using Commerce.Application.Database;
using Stripe;

namespace Commerce.Application
{
    public class CommerceApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)        
        {
            // Context and Unit of Work
            builder.RegisterType<PushMarketContext>()
                .As<PushMarketContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EfUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            RegisterDatabaseRepositories(builder);

            // Payment Processors
            RegisterPaymentProcessors(builder);

            // Email
            RegisterEmailComponents(builder);

            // Cart 
            builder.RegisterType<CartIdentificationService>().As<ICartIdentificationService>();
            builder.RegisterType<CartManagementService>().As<ICartManagementService>();
            builder.RegisterType<CartRepository>().As<ICartRepository>();

            // Analytics
            builder.RegisterType<AnalyticsCollector>().As<IAnalyticsCollector>();
            builder.RegisterType<AnalyticsAggregator>().As<IAnalyticsAggregator>();

            // HttpContext
            builder.Register<HttpContextBase>(c => new HttpContextWrapper(HttpContext.Current));

            // Azure Infrastructure
            var azureHosted = ConfigurationManager.AppSettings["AzureHosted"].ToBoolTryParse();

            if (azureHosted)
            {
                RegisterAzureComponents(builder);
            }
        }

        private static void RegisterDatabaseRepositories(ContainerBuilder builder)
        {
            // User Repositories
            builder.RegisterType<AggregateReadOnlyRepository>()
                .As<IReadOnlyAggregateUserRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AggregateWriteableRepository>()
                .As<IWritableAggregateUserRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<MembershipReadableRepository>().As<IMembershipReadOnlyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipWritableRepository>().As<IMembershipWritableRepository>().InstancePerLifetimeScope();

            // List Repositories
            builder.RegisterType<JsonCategoryRepository>().As<IJsonCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JsonSizeRepository>().As<IJsonSizeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JsonBrandRepository>().As<IJsonBrandRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JsonColorRepository>().As<IJsonColorRepository>().InstancePerLifetimeScope();

            // Product Repositories
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();
            builder.RegisterType<InventoryRepository>().As<IInventoryRepository>().InstancePerLifetimeScope();

            // Order Stuff
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderManager>().As<IOrderManager>().InstancePerLifetimeScope();

            // Resource Repositories
            builder.RegisterType<FileResourceRepository>().As<IFileResourceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ImageBundleRepository>().As<IImageBundleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ImageProcessor>().As<IImageProcessor>().InstancePerLifetimeScope();
            builder.RegisterType<BlankImageRepository>().As<IBlankImageRepository>().InstancePerLifetimeScope();

            // Generic Repositories
            builder.RegisterType<EFGenericRepository<Brand>>().As<IGenericRepository<Brand>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Color>>().As<IGenericRepository<Color>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Category>>().As<IGenericRepository<Category>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Size>>().As<IGenericRepository<Size>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<SizeGroup>>()
                .As<IGenericRepository<SizeGroup>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Product>>().As<IGenericRepository<Product>>().InstancePerLifetimeScope();
        }

        private static void RegisterPaymentProcessors(ContainerBuilder builder)
        {
            builder.RegisterType<StripeConfigAdapter>().As<IStripeConfigAdapter>();
            builder
                .Register(c => new StripeChargeService(c.Resolve<IStripeConfigAdapter>().SecretKey))
                .As<StripeChargeService>();
            if (Payment.StripeConfiguration.Settings.ServerSideMockEnabled.ToBoolTryParse())
            {
                builder.RegisterType<MockPaymentProcessor>().As<IPaymentProcessor>();
            }
            else
            {
                builder.RegisterType<StripePaymentProcessor>().As<IPaymentProcessor>();
            }            
        }

        private static void RegisterEmailComponents(ContainerBuilder builder)
        {
            // Email Functionality
            builder.Register(ctx => EmailConfigAdapter.Settings).As<IEmailConfigAdapter>();
            builder.RegisterType<CustomerEmailBuilder>().As<ICustomerEmailBuilder>();
            builder.RegisterType<AdminEmailBuilder>().As<IAdminEmailBuilder>();
            builder.RegisterType<EmbeddedResourceRepository>().As<IEmbeddedResourceRepository>();
            builder.RegisterType<TemplateEngine>().As<ITemplateEngine>();
            builder.RegisterType<WebAppTemplateLocator>().As<ITemplateLocator>();

            if (EmailConfigAdapter.Settings.ServerSideMockEnabled.ToBoolTryParse())
            {
                builder.RegisterType<MockEmailService>().As<IEmailService>();
            }
            else
            {
                builder.RegisterType<EmailService>().As<IEmailService>();
            }
        }

        public static void RegisterAzureComponents(ContainerBuilder builder)
        {
            var configuration = AzureConfiguration.Settings;

            // File Resource Repositories - intentionally overwrite the local file system-based storage
            builder.Register(ctx => new AzureFileResourceRepository(
                        configuration.StorageConnectionString,
                        configuration.ResourcesStorageContainer,
                        ctx.Resolve<PushMarketContext>()))
                .As<IFileResourceRepository>()
                .InstancePerLifetimeScope();            
        }
    }
}
