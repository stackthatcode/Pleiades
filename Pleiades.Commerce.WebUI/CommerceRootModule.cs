using System;
using Autofac;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Web;
using Pleiades.Framework.Web.Security.Aspect;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.WebUI.Plumbing.Autofac;
using Pleiades.Commerce.WebUI.Plumbing.Security;

namespace Pleiades.Commerce.WebUI
{
    public class CommerceRootModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacContainer>().As<IGenericContainer>();

            // Security Plumbing
            builder.RegisterType<CommerceAuthorizationRule>().As<PleiadesAuthorizeRule>();
            builder.RegisterType<CommerceSecurityCodeResponder>().As<PostbackSecurityResponder>();

            // External Modules
            builder.RegisterModule<WebModule>();
            builder.RegisterModule<PersistModule>();
        }
    }
}