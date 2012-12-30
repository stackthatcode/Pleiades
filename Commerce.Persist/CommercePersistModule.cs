using System;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Injection;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Commerce.Domain.Interfaces;
using Commerce.Persist.Security;
using Commerce.Persist.Products;

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
            builder.RegisterType<SectionRepository>().As<ISectionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SizeGroupRepository>().As<ISizeGroupRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SizeRepository>().As<ISizeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ColorRepository>().As<IColorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BrandRepository>().As<IBrandRepository>().InstancePerLifetimeScope();
        }
    }
}