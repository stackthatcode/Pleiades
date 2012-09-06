using System;
using Pleiades.Execution;

namespace Pleiades.UnitTests.Execution
{
    public class ContextStub : IStepContext
    {
        public ContextStub()
        {
            this.IsExecutionStateValid = true;
        }

        public virtual bool IsExecutionStateValid { get; set; }
        public virtual string PropertyA { get; set; }
        public virtual int PropertyB { get; set; }
    }
}