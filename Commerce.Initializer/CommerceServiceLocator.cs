using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Pleiades.Injection;
using Commerce.WebUI;

namespace Commerce.Initializer
{
    public class CommerceServiceLocator
    {
        ILifetimeScope LifetimeScope;

        private CommerceServiceLocator(ILifetimeScope lifetimeScope)
        {
            this.LifetimeScope = lifetimeScope;
        }

        public static CommerceServiceLocator Create()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceWebUIModule>();
            var container = builder.Build();
            var lifetimeScope = container.BeginLifetimeScope();
            return new CommerceServiceLocator(lifetimeScope);
        }

        public T Resolve<T>()
        {
            return LifetimeScope.Resolve<T>();
        }

        public T ResolveKeyed<T>(object key)
        {
            return LifetimeScope.ResolveKeyed<T>(key);
        }
    }
}
