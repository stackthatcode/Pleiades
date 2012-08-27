namespace Pleiades.Execution
{
    public interface IStepContext
    {
        bool IsExecutionStateValid { get; set; }
    }
}
