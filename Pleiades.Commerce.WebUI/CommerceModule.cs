using System;
using Autofac;
using Pleiades.Framework.Autofac;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.Web;

namespace Pleiades.Commerce.WebUI
{
    public class CommerceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var genericBuilder = new GenericBuilder(builder);

            PersistRegistration.Register(genericBuilder);
            WebRegistration.Register (genericBuilder);            
        }
    }
}