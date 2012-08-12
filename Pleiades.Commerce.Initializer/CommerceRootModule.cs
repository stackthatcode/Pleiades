using System;
using Autofac;
using Pleiades.Framework.Injection;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.Web;

namespace Pleiades.Commerce.Initializer
{
    public class CommerceRootModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacContainer>().As<IGenericContainer>();

            builder.RegisterModule<WebModule>();
            builder.RegisterModule<PersistModule>();
        }
    }
}