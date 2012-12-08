using System;
using Autofac;
using Pleiades.Injection;

namespace Commerce.WebUI.Plumbing
{
    public class AutofacContainer : IContainerAdapter
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