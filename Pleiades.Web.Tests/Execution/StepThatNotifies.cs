using Pleiades.Execution;

namespace Pleiades.UnitTests.Execution
{
    public class StepThatNotifies : Step<ContextStub>
    {
        object NotifyPayload { get; set; }

        public StepThatNotifies(object notifyPayload)
        {
            this.NotifyPayload = notifyPayload;
        }

        public override ContextStub Execute(ContextStub context)
        {
            this.Notify(this.NotifyPayload);
            return context;
        }
    }
}
