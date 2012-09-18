using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Execution
{
    // How to reuse Composites without creating a Step AND a Composite -- create one object and *run* with it
    // 1.) Specify the IStep<T> implementation
    // 2.) 

    //public class ConditionalStep<T> : IStep<T> where T : IStepContext
    //{
    //    public IStep<T> Condition { get; set; }

    //    public ConditionalStep(IStep<T> condition)
    //    {
    //        this.Condition = condition;
    //    }

    //    public T Execute(T context)
    //    {
    //        var resultContext = this.Condition.Execute(context);
    //        if (!result.IsExecutionStateValid)
    //            return result;

    //    }
    //}
}
