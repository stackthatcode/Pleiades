using System;
using Autofac;

namespace Pleiades.Framework.Autofac
{
    public class GenericBuilder : Injection.IGenericBuilder
    {
        ContainerBuilder _Builder;

        public GenericBuilder(ContainerBuilder builder)
        {
            this._Builder = builder;
        }

        public void RegisterType<T>()
        {
            this._Builder.RegisterType<T>();
        }

        public void RegisterTypeAs<TConcrete, TBase>()
        {
            this._Builder.RegisterType<TConcrete>().As<TBase>();                
        }
    }
}
