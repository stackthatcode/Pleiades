using System;
using Autofac;

namespace Pleiades.Framework.Autofac
{
    public class Builder : Injection.IGenericBuilder
    {
        ContainerBuilder Builder;

        public Builder(ContainerBuilder builder)
        {
            this.Builder = builder;
        }

        public void RegisterType<T>()
        {
            this.Builder.RegisterType<T>();
        }
    }
}
