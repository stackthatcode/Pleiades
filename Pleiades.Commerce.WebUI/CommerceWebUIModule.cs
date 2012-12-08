using System;
using Autofac;
using Pleiades.Injection;
using Pleiades.Web.Security;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Interface;
using Autofac;
using Autofac.Integration.Mvc;
using Commerce.Persist;
using Commerce.WebUI.Plumbing;

namespace Commerce.WebUI
{
    public class CommerceWebUIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Autofac
            builder.RegisterType<AutofacContainer>().As<IContainerAdapter>().InstancePerLifetimeScope();

            // External Modules
            builder.RegisterModule<WebSecurityAggregateModule>();
            builder.RegisterModule<CommercePersistModule>();

            // Aggregrate User Registration framework
            WebSecurityAggregateBroker.RegisterSecurityContextFactory<SecurityContextFactory>();
            WebSecurityAggregateBroker.RegisterSecurityResponder<SecurityResponder>();
            WebSecurityAggregateBroker.Build(builder);

            // Register Commerce Controllers
            builder.RegisterControllers(typeof(CommerceHttpApplication).Assembly);
        }
    }
}