using System;
using System.Data.Entity;
using Autofac;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Data.EF;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Commerce.Persist.Security;

namespace Pleiades.Commerce.Persist
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
            builder.RegisterType<IdentityUserRepository>().As<IIdentityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipRepository>().As<IMembershipRepository>().InstancePerLifetimeScope();

            // TODO: add the Products Repository
        }
    }
}