using System;
using Pleiades.Framework.Execution;

namespace Pleiades.Framework.UnitTests.Execution
{
    public class ContextStub : IStepContext
    {
        public ContextStub()
        {
            this.ExecutionStateValid = true;
        }

        public virtual bool ExecutionStateValid { get; set; }
        public virtual string PropertyA { get; set; }
        public virtual int PropertyB { get; set; }
    }
}