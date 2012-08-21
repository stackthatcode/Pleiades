using System;
using Autofac;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Web;
using Pleiades.Framework.Web.Security.Aspect;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.WebUI.Plumbing.Autofac;
using Pleiades.Commerce.WebUI.Plumbing.Security;

namespace Pleiades.Commerce.WebUI
{
    public class CommerceWebUIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Autofac
            builder.RegisterType<AutofacContainer>().As<IGenericContainer>().InstancePerLifetimeScope();

            // External Modules
            FrameworkWebModule.RegisterAuthorizationRule<MySystemAuthContextBuilder>();
            FrameworkWebModule.RegisterPostbackResponder<MySecurityCodeResponder>();
            builder.RegisterModule<FrameworkWebModule>();
            builder.RegisterModule<CommercePersistModule>();
        }
    }
}