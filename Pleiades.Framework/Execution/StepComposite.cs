using System.Collections.Generic;
using Pleiades.Framework.Helpers;

namespace Pleiades.Framework.Execution
{
    public class StepComposite<TContext> : Step<TContext>
            where TContext : IStepContext
    {
        protected List<Step<TContext>> Steps = new List<Step<TContext>>();

        public void Add(Step<TContext> step)
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
        public override void Execute(TContext context)
        {
            if (!context.ExecutionStateValid)
            {
                return;
            }

            foreach (var step in Steps)
            {
                step.Execute(context);

                // If the ExecutionState is no longer valid, then stop!
                if (!context.ExecutionStateValid)
                {
                    return;
                }
            }
        }
    }
}
