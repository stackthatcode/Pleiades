namespace Pleiades.Framework.Execution
{
    public interface IStepContext
    {
        bool ExecutionStateValid { get; set; }
    }
}
