using System.Data.Entity;
using Autofac;
using Pleiades.Application.Data;
using Pleiades.Application;
using Pleiades.Application.Data.EF;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Commerce.Application.Concrete.Infrastructure;
using Commerce.Application.Concrete.Lists;
using Commerce.Application.Concrete.Orders;
using Commerce.Application.Concrete.Products;
using Commerce.Application.Concrete.Security;
using Commerce.Application.Database;
using Commerce.Application.Concrete;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Lists;
using Commerce.Application.Model.Products;

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

            // Order ReadOnlyRepository
            builder.RegisterType<OrderSubmissionService>().As<IOrderSubmissionService>().InstancePerLifetimeScope();
            
            // Resource Repositories
            builder.RegisterType<FileResourceRepository>().As<IFileResourceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ImageBundleRepository>().As<IImageBundleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ImageProcessor>().As<IImageProcessor>().InstancePerLifetimeScope();

            // Generic Repositories
            builder.RegisterType<EFGenericRepository<Brand>>().As<IGenericRepository<Brand>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Color>>().As<IGenericRepository<Color>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Category>>().As<IGenericRepository<Category>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Size>>().As<IGenericRepository<Size>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<SizeGroup>>().As<IGenericRepository<SizeGroup>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Product>>().As<IGenericRepository<Product>>().InstancePerLifetimeScope();

            // Payment Processors
            builder.RegisterType<GetPaidPaymentProcessor>().As<IPaymentProcessor>();

            // Email Repositories
            builder.RegisterType<EmailGenerator>().As<IEmailGenerator>();
            builder.RegisterType<EmailService>().As<IEmailService>();

            // Analytic Service
            builder.RegisterType<AnalyticService>().As<IAnalyticsService>();
        }
    }
}
