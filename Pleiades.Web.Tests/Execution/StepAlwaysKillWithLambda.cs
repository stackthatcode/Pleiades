using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Framework.Execution;

namespace Pleiades.Framework.UnitTests.Execution
{
    public class StepAlwaysKillWithLambda : Step<ContextStub>
    {
        public override void Execute(ContextStub context)
        {
            this.Kill(context, () => context.PropertyA = "Oh my!");
        }
    }
}
