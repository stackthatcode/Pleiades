﻿namespace Pleiades.App.Injection
{
    public interface IContainerAdapter
    {
        T Resolve<T>();
        T ResolveKeyed<T>(object key);
    }
}