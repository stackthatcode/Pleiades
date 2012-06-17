namespace Pleiades.Framework.Injection
{
    public interface Container
    {
        T Resolve<T>();
    }
}