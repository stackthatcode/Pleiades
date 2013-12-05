using System;
using Autofac;

namespace Pleiades.App.Injection
{
    public class AutofacContainer : IContainerAdapter
    {
        public Guid Tracer = Guid.NewGuid();
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