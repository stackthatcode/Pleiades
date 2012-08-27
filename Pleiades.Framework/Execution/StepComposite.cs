using System.Collections.Generic;
using Pleiades.Helpers;
using Pleiades.Injection;

namespace Pleiades.Execution
{
    public class StepComposite<TContext> : Step<TContext>
            where TContext : IStepContext
    {
        protected IGenericContainer Container { get; set; }
        public List<Step<TContext>> Steps { get; private set; }

        public StepComposite(IGenericContainer container)
        {
            this.Steps = new List<Step<TContext>>();
            this.Container = container;
        }

        public void Inject<TChildStep>() where TChildStep : Step<TContext>
        {
            var step = this.Container.Resolve<TChildStep>();
            this.Steps.Add(step);
        }

        public void Register(Step<TContext> step)
        {
            this.Steps.Add(step);
        }

        public override void Attach(IStepObserver observer)
        {
            this.Steps.ForEach(x => x.Attach(observer));
        }

        public override void Notify(object o)
        {
            this.Steps.ForEach(x => x.Notify(o));
        }

        // Executes a sequence of Steps
        public override TContext Execute(TContext context)
        {
            if (!context.IsExecutionStateValid)
            {
                return context;
            }

            foreach (var step in Steps)
            {
                step.Execute(context);

                // If the ExecutionState is no longer valid, then stop!
                if (!context.IsExecutionStateValid)
                {
                    return context;
                }
            }

            return context;
        }
    }
}
