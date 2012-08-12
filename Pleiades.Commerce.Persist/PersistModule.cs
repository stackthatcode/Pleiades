using System;
using System.Data.Entity;
using Autofac;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Data.EF;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Commerce.Persist.Users;
using Pleiades.Commerce.Domain;
using Pleiades.Commerce.Domain.Interface;

namespace Pleiades.Commerce.Persist
{
    public class PersistModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DomainModule>();

            // Context and Unit of Work
            builder.RegisterType<PleiadesContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<EFUnitOfWork>().InstancePerLifetimeScope();

            // Domain-serving Repositories
            builder.RegisterType<AggregateUserRepository>().As<IAggregateUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<IdentityUserRepository>().As<IIdentityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipRepository>().As<IMembershipRepository>().InstancePerLifetimeScope();

            // TODO: add the Products Repository
        }
    }
}
