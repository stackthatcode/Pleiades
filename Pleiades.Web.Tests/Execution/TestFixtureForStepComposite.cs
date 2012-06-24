using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using NUnit.Framework;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;

namespace Pleiades.Framework.UnitTests.Execution
{
    [TestFixture]
    public class TestFixtureForStepComposite
    {
        [Test]
        public void StepComposite_Inject_Invokes_Resolve_On_Container()
        {
            // Arrange
            var container = MockRepository.GenerateMock<IContainer>();
            container.Expect(x => x.Resolve<StepOk>()).Return(new StepOk());            
            var step = new StepComposite<ContextStub>(container);

            // Act
            step.Inject<StepOk>();
            
            // Assert
            container.VerifyAllExpectations();
        }

        [Test]
        public void StepComposite_Executes_All_Steps()
        {
            // Arrange 
            var container = MockRepository.GenerateStub<IContainer>();
            var stepComposite = new StepComposite<ContextStub>(container);
            var context = new ContextStub();

            var step1 = MockRepository.GenerateMock<Step<ContextStub>>();
            var step2 = MockRepository.GenerateMock<Step<ContextStub>>();
            var step3 = MockRepository.GenerateMock<Step<ContextStub>>();

            step1.Expect(x => x.Execute(context));
            step2.Expect(x => x.Execute(context));
            step3.Expect(x => x.Execute(context));

            stepComposite.Register(step1);
            stepComposite.Register(step2);
            stepComposite.Register(step3);

            // Act
            stepComposite.Execute(context);

            // Assert
            step1.VerifyAllExpectations();
        }

        [Test]
        public void StepComposite_Executes_All_Steps_Until_Kill()
        {
            // Arrange 
            var container = MockRepository.GenerateStub<IContainer>();
            var stepComposite = new StepComposite<ContextStub>(container);
            var context = new ContextStub();

            var step1 = new StepOk();
            var step2 = new StepOk();
            var step3 = new StepAlwaysKill();
            var step4 = MockRepository.GenerateMock<Step<ContextStub>>();
            var step5 = MockRepository.GenerateMock<Step<ContextStub>>();

            stepComposite.Register(step1);
            stepComposite.Register(step2);
            stepComposite.Register(step3);
            stepComposite.Register(step4);
            stepComposite.Register(step5);

            // Act
            stepComposite.Execute(context);

            // Assert
            step4.VerifyAllExpectations();
            step5.VerifyAllExpectations();
        }

        // TODO: verify that Observers are Attached to all Steps
    }
}
