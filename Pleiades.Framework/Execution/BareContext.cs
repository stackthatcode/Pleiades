namespace Pleiades.Framework.Execution
{
    public class BareContext : IStepContext
    {
        public bool IsExecutionStateValid { get; set; }

        public BareContext()
        {
            this.IsExecutionStateValid = true;
        }
    }
}
