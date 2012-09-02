using System;
using Autofac;
using Pleiades.Injection;
using Pleiades.Web;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Execution.Composites;
using Commerce.Persist;
using Commerce.WebUI.Plumbing.Autofac;
using Commerce.WebUI.Plumbing.Security;

namespace Commerce.WebUI
{
    public class CommerceWebUIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Autofac
            builder.RegisterType<AutofacContainer>().As<IGenericContainer>().InstancePerLifetimeScope();

            // External Modules
            WebInjectionBroker.RegisterAuthContextBuilder<MySystemAuthContextBuilder>();
            WebInjectionBroker.RegisterPostbackResponder<MySecurityCodeResponder>();
            WebInjectionBroker.RegisterSystemAuthorizationStep<SystemAuthorizationComposite>();

            builder.RegisterModule<WebModule>();
            builder.RegisterModule<CommercePersistModule>();
        }
    }
}