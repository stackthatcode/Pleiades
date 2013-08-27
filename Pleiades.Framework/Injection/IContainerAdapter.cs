namespace Pleiades.Application.Injection
{
    public interface IContainerAdapter
    {
        T Resolve<T>();
        T ResolveKeyed<T>(object key);
    }
}