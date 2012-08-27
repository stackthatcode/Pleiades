using System;
using Pleiades.Execution;

namespace Pleiades.UnitTests.Execution
{
    public class StepOk : Step<ContextStub>
    {
        public override ContextStub Execute(ContextStub context)
        {
            context.PropertyB = 999;
            return context;
        }
    }
}
