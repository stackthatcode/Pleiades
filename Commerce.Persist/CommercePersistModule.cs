using System;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Injection;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Persist.Lists;
using Commerce.Persist.Resources;
using Commerce.Persist.Security;

namespace Commerce.Persist
{
    public class CommercePersistModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Context and Unit of Work
            builder.RegisterType<PleiadesContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // User Repositories
            builder.RegisterType<AggregateUserRepository>().As<IAggregateUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PfMembershipRepository>().As<IMembershipProviderRepository>().InstancePerLifetimeScope();

            // List Repositories
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SizeRepository>().As<ISizeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BrandRepository>().As<IBrandRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ColorRepository>().As<IColorRepository>().InstancePerLifetimeScope();

            // Resource Repositories
            builder.RegisterType<ImageBundleRepository>().As<IImageBundleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FileResourceRepository>().As<IFileResourceRepository>().InstancePerLifetimeScope();

            // Generic Domain-serving Repositories
            builder.RegisterType<EFGenericRepository<Category>>().As<IGenericRepository<Category>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<Size>>().As<IGenericRepository<Size>>().InstancePerLifetimeScope();
            builder.RegisterType<EFGenericRepository<SizeGroup>>().As<IGenericRepository<SizeGroup>>().InstancePerLifetimeScope();
        }
    }
}