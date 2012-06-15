namespace Pleiades.Framework.Execution
{
    public interface IStep
    {
        void Execute(IStepContext context);
        void Attach(IStepObserver observer);
        void Notify(object o);
    }
}
