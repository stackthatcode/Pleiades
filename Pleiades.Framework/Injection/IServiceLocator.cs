namespace Pleiades.Injection
{
    public interface IServiceLocator
    {
        T Resolve<T>();
        T ResolveKeyed<T>(object key);
    }
}