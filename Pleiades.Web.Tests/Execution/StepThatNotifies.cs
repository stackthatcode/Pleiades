using Pleiades.Framework.Execution;

namespace Pleiades.Framework.UnitTests.Execution
{
    public class StepThatNotifies : Step<ContextStub>
    {
        object NotifyPayload { get; set; }

        public StepThatNotifies(object notifyPayload)
        {
            this.NotifyPayload = notifyPayload;
        }

        public override void Execute(ContextStub context)
        {
            this.Notify(this.NotifyPayload);
        }
    }
}
