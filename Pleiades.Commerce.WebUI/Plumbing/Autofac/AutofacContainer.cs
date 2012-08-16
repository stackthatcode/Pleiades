using System;
using Autofac;
using Pleiades.Framework.Injection;

namespace Pleiades.Commerce.WebUI.Plumbing.Autofac
{
    public class AutofacContainer : IGenericContainer
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
    }
}