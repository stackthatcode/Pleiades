using System;
using Autofac;
using Pleiades.Injection;
using Pleiades.Web.Autofac;
using Pleiades.Web.Security;
using Commerce.Persist;

namespace Commerce.WebUI
{
    public class CommerceInitializerModules : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Autofac
            builder.RegisterType<AutofacContainer>().As<IContainerAdapter>().InstancePerLifetimeScope();

            // External Modules
            builder.RegisterModule<WebSecurityAggregateModule>();
            builder.RegisterModule<CommercePersistModule>();
        }
    }
}