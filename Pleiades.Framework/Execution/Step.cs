using System;
using System.Collections.Generic;
using Pleiades.Framework.Helpers;

namespace Pleiades.Framework.Execution
{
    public abstract class Step<TContext> where TContext : IStepContext
    {
        protected List<IStepObserver> Observers = new List<IStepObserver>();

        /// <summary>
        /// Steps receive the Context to use for Validation and for affecting State changes
        /// Returns true if the Step was Valid and Executed a State Change
        /// </summary>
        public abstract TContext Execute(TContext context);

        // Subscribes an observer to listen for Notifications
        public virtual void Attach(IStepObserver observer)
        {
            this.Observers.Add(observer);
        }

        // Broadcast notification to all subscribed Observers
        public virtual void Notify(object o)
        {
            this.Observers.ForEach(x => x.Notify(o));
        }

        public virtual TContext Kill(TContext context)
        {
            context.IsExecutionStateValid = false;
            return context;
        }

        public virtual TContext Kill(TContext context, Action action)
        {
            this.Kill(context);
            action();
            return context;
        }
    }
}
