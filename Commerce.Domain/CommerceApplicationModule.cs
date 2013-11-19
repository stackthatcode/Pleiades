using System.Data.Entity;
using System.Web;
using Autofac;
using Commerce.Application.Analytics;
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
using Pleiades.Application.Data;
using Pleiades.Application.Data.EF;
using Pleiades.Application.Utility;
using Pleiades.Web.Security.Interface;
using Commerce.Application.Database;
using Stripe;

namespace Commerce.Application
{
    public class CommerceApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)        {
            // Context and Unit of Work
            builder.RegisterType<PushMarketContext>()
                .As<PushMarketContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EfUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            // User Repositories
            builder.RegisterType<AggregateReadOnlyRepository>().As<IReadOnlyAggregateUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AggregateWriteableRepository>().As<IWritableAggregateUserRepository>().InstancePerLifetimeScope();
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
            builder.RegisterType<EFGenericRepository<SizeGroup>>().As<IGenericRepository<SizeGroup>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Product>>().As<IGenericRepository<Product>>().InstancePerLifetimeScope();

            // Payment Processors
            builder.RegisterType<StripeConfigAdapter>().As<IStripeConfigAdapter>();
            builder
                .Register(c => new StripeChargeService(c.Resolve<IStripeConfigAdapter>().SecretKey))
                .As<StripeChargeService>();
            if (Payment.StripeConfiguration.Settings.MockServiceEnabled.ToBoolTryParse())
            {
                builder.RegisterType<MockPaymentProcessor>().As<IPaymentProcessor>();
            }
            else
            {
                builder.RegisterType<StripePaymentProcessor>().As<IPaymentProcessor>();
            }

            // Email Repositories
            builder.Register(ctx => EmailConfigAdapter.Settings).As<IEmailConfigAdapter>();
            builder
                .RegisterType<CustomerEmailBuilder>()
                .Keyed<IAdminEmailBuilder>(EmailBuilderType.Customer);
            builder
                .RegisterType<AdminEmailBuilder>()
                .Keyed<IAdminEmailBuilder>(EmailBuilderType.Admin);
            
            if (EmailConfigAdapter.Settings.MockServiceEnabled.ToBoolTryParse())
            {
                builder.RegisterType<MockEmailService>().As<IEmailService>();
            }
            else
            {
                builder.RegisterType<EmailService>().As<IEmailService>();                                   
            }

            // Cart 
            builder.RegisterType<CartIdentificationService>().As<ICartIdentificationService>();
            builder.RegisterType<CartManagementService>().As<ICartManagementService>();
            builder.RegisterType<CartRepository>().As<ICartRepository>();

            // Analytics
            builder.RegisterType<AnalyticsCollector>().As<IAnalyticsCollector>();
            builder.RegisterType<AnalyticsAggregator>().As<IAnalyticsAggregator>();

            // HttpContext
            builder.Register<HttpContextBase>(c => new HttpContextWrapper(HttpContext.Current));
        }
    }
}
