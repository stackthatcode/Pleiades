using System;
using Autofac;
using Pleiades.Framework.Injection;

namespace Pleiades.Framework.Autofac
{
    public class Container : Injection.IGenericContainer
    {
        ILifetimeScope LifetimeScope { get; set; }

        public Container(ILifetimeScope lifetimeScope)
        {
            this.LifetimeScope = lifetimeScope;
        }

        public T Resolve<T>()
        {
            return this.LifetimeScope.Resolve<T>();
        }
    }
}
