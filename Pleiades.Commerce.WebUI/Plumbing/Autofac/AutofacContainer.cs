using System;
using Autofac;
using Pleiades.Injection;

namespace Commerce.WebUI.Plumbing.Autofac
{
    public class AutofacContainer : IServiceLocator
    {
        ILifetimeScope LifetimeScope { get; set; }

        public AutofacContainer(ILifetimeScope lifetimeScope)
        {
            this.LifetimeScope = lifetimeScope;
        }

        public T Resolve<T>()
        {
            return this.LifetimeScope.Resolve<T>();
        }

        public T ResolveKeyed<T>(object key)
        {
            return this.LifetimeScope.ResolveKeyed<T>(key);
        }
    }
}