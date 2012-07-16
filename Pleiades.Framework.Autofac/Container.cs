using System;
using Autofac;
using Pleiades.Framework.Injection;

namespace Pleiades.Framework.Autofac
{
    public class Container : Injection.IGenericContainer
    {
        IContainer AutofacContainer { get; set; }

        public Container(IContainer autofacContainer)
        {
            this.AutofacContainer = autofacContainer;
        }

        public T Resolve<T>()
        {
            return this.AutofacContainer.Resolve<T>();
        }
    }
}
