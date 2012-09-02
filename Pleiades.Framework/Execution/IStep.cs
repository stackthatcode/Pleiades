namespace Pleiades.Execution
{
    public interface IStep<TContext> where TContext : IStepContext
    {
        TContext Execute(TContext context);
    }
}