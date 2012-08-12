using System;
using Autofac;
using Pleiades.Framework.Injection;
using Pleiades.Commerce.Domain.Concrete;
using Pleiades.Commerce.Domain.Interface;

namespace Pleiades.Commerce.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AggregateUserService>().As<IAggregateUserService>().InstancePerLifetimeScope();
        }
    }
}
