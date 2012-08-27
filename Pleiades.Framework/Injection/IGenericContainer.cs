namespace Pleiades.Injection
{
    public interface IGenericContainer
    {
        T Resolve<T>();
        T ResolveKeyed<T>(object key);
    }
}