using System;
using Pleiades.Execution;

namespace Pleiades.UnitTests.Execution
{
    public class StepXYZ : Step<ContextStub>
    {
        public override ContextStub Execute(ContextStub context)
        {
            context.PropertyB = 800000;
            return context;
        }
    }
}
