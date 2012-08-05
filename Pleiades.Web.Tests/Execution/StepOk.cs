using System;
using Pleiades.Framework.Execution;

namespace Pleiades.Framework.UnitTests.Execution
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
