using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Framework.Injection;

namespace Pleiades.Framework.Execution
{
    public class InsertedStepFactory<TContext, TInsertedStep> 
            where TContext : IStepContext
            where TInsertedStep : Step<TContext>
    {
        IContainer Container { get; set; }

        public InsertedStepFactory(IContainer container)
        {
            this.Container = container;
        }
        
        public StepComposite<TContext> Make<TStep>() where TStep : Step<TContext>
        {
            var step = new StepComposite<TContext>(this.Container);
            step.Inject<TInsertedStep>();
            step.Inject<TStep>();
            return step;
        }
    } 
}
