using System;
using Pleiades.Framework.Execution;

namespace Pleiades.Framework.UnitTests.Execution
{
    public class StepOk : Step<ContextStub>
    {
        public override void Execute(ContextStub context)
        {
            context.PropertyB = 999;
        }
    }
}
