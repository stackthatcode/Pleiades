namespace Pleiades.Framework.Execution
{
    public interface IStepContext
    {
        bool IsExecutionStateValid { get; set; }
    }
}
