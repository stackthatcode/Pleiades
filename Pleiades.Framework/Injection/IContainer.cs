namespace Pleiades.Framework.Injection
{
    public interface IContainer
    {
        T Resolve<T>();
    }
}