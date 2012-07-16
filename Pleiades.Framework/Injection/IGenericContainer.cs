namespace Pleiades.Framework.Injection
{
    public interface IGenericContainer
    {
        T Resolve<T>();
    }
}