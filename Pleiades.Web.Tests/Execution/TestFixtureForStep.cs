using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using NUnit.Framework;
using Pleiades.Framework.Execution;

namespace Pleiades.Framework.UnitTests.Execution
{
    [TestFixture]
    public class TestFixtureForStep
    {
        [Test]
        public void No_Kill_Leaves_ExecutionStateValid_True()
        {
            var step = new StepOk();
            var context = new ContextStub();

            Assert.IsTrue(context.IsExecutionStateValid);
            step.Execute(context);
            Assert.IsTrue(context.IsExecutionStateValid);
        }

        [Test]
        public void KillPipeline_Sets_ExecutionStateValid_To_False()
        {
            var step = new StepAlwaysKill();
            var context = new ContextStub();

            Assert.IsTrue(context.IsExecutionStateValid);
            step.Execute(context);
            Assert.IsFalse(context.IsExecutionStateValid);
        }

        [Test]
        public void KillPipeline_Overload_Sets_ExecutionStateValid_To_False()
        {
            var step = new StepAlwaysKillWithLambda();
            var context = new ContextStub();

            Assert.IsTrue(context.IsExecutionStateValid);
            step.Execute(context);
            Assert.IsFalse(context.IsExecutionStateValid);
        }

        [Test]
        public void All_Subscribed_Observers_Are_Notified()
        {
            // Arrange
            var notifyPayload = new object();
            var step = new StepThatNotifies(notifyPayload);

            var observer1 = MockRepository.GenerateMock<IStepObserver>();
            var observer2 = MockRepository.GenerateMock<IStepObserver>();
            var observer3 = MockRepository.GenerateMock<IStepObserver>();

            observer1.Expect(x => x.Notify(notifyPayload));
            observer2.Expect(x => x.Notify(notifyPayload));
            observer3.Expect(x => x.Notify(notifyPayload));

            step.Attach(observer1);
            step.Attach(observer2);
            step.Attach(observer3);

            // Act
            step.Execute(new ContextStub());

            // Assert
            observer1.VerifyAllExpectations();
            observer2.VerifyAllExpectations();
            observer3.VerifyAllExpectations();
        }

    }
}
