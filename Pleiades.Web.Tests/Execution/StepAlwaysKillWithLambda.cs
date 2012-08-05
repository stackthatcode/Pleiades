using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Framework.Execution;

namespace Pleiades.Framework.UnitTests.Execution
{
    public class StepAlwaysKillWithLambda : Step<ContextStub>
    {
        public override ContextStub Execute(ContextStub context)
        {
            return this.Kill(context, () => context.PropertyA = "Oh my!");
        }
    }
}
