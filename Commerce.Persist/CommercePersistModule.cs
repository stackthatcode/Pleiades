using System;
using System.Data.Entity;
using Autofac;
using Pleiades.Injection;
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
            builder.RegisterType<EFUnitOfWork>().InstancePerLifetimeScope();

            // Domain-serving Repositories
            builder.RegisterType<AggregateUserRepository>().As<IAggregateUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PfMembershipRepository>().As<IMembershipProviderRepository>().InstancePerLifetimeScope();

            // TODO: add the Products Repository
        }
    }
}