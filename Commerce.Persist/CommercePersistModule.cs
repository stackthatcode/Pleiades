using System;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Injection;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Interface;
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

            // Domain-serving Repositories
            builder.RegisterType<AggregateUserRepository>().As<IAggregateUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PfMembershipRepository>().As<IMembershipProviderRepository>().InstancePerLifetimeScope();

            // TODO: add the Products Repository, Category Repository
        }
    }
}