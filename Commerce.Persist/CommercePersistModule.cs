using System;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Injection;
using Pleiades.Web.Security.Interface;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Products;
using Commerce.Persist.Lists;
using Commerce.Persist.Products;
using Commerce.Persist.Resources;
using Commerce.Persist.Security;

namespace Commerce.Persist
{
    public class CommercePersistModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Context and Unit of Work
            builder.RegisterType<PleiadesContext>()
                .As<PleiadesContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // User Repositories
            builder.RegisterType<AggregateUserRepository>().As<IAggregateUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PfMembershipRepository>().As<IMembershipProviderRepository>().InstancePerLifetimeScope();

            // List Repositories
            builder.RegisterType<JsonCategoryRepository>().As<IJsonCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JsonSizeRepository>().As<IJsonSizeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JsonBrandRepository>().As<IJsonBrandRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JsonColorRepository>().As<IJsonColorRepository>().InstancePerLifetimeScope();

            // Product Repositories
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductSearchRepository>().As<IProductSearchRepository>().InstancePerLifetimeScope();
            
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
        }
    }
}