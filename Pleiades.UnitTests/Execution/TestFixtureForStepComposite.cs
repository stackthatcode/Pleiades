using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using NUnit.Framework;
using Pleiades.Execution;
using Pleiades.Injection;

namespace Pleiades.UnitTests.Execution
{
    [TestFixture]
    public class TestFixtureForStepComposite
    {
        [Test]
        public void StepComposite_Inject_Invokes_Resolve_On_Container()
        {
            // Arrange
            var container = MockRepository.GenerateMock<IServiceLocator>();
            container.Expect(x => x.Resolve<StepOk>()).Return(new StepOk());            
            var step = new CompositeStep<ContextStub>(container);

            // Act
            step.ResolveAndAdd<StepOk>();
            
            // Assert
            container.VerifyAllExpectations();
        }

        [Test]
        public void StepComposite_Executes_All_Steps()
        {
            // Arrange 
            var container = MockRepository.GenerateStub<IServiceLocator>();
            var stepComposite = new CompositeStep<ContextStub>(container);
            var context = new ContextStub();

            var step1 = MockRepository.GenerateMock<Step<ContextStub>>();
            var step2 = MockRepository.GenerateMock<Step<ContextStub>>();
            var step3 = MockRepository.GenerateMock<Step<ContextStub>>();

            step1.Expect(x => x.Execute(context));
            step2.Expect(x => x.Execute(context));
            step3.Expect(x => x.Execute(context));

            stepComposite.Add(step1);
            stepComposite.Add(step2);
            stepComposite.Add(step3);

            // Act
            stepComposite.Execute(context);

            // Assert
            step1.VerifyAllExpectations();
        }

        [Test]
        public void StepComposite_Executes_All_Steps_Until_Kill()
        {
            // Arrange 
            var container = MockRepository.GenerateStub<IServiceLocator>();
            var stepComposite = new CompositeStep<ContextStub>(container);
            var context = new ContextStub();

            var step1 = new StepOk();
            var step2 = new StepOk();
            var step3 = new StepAlwaysKill();
            var step4 = MockRepository.GenerateMock<Step<ContextStub>>();
            var step5 = MockRepository.GenerateMock<Step<ContextStub>>();

            stepComposite.Add(step1);
            stepComposite.Add(step2);
            stepComposite.Add(step3);
            stepComposite.Add(step4);
            stepComposite.Add(step5);

            // Act
            stepComposite.Execute(context);

            // Assert
            step4.VerifyAllExpectations();
            step5.VerifyAllExpectations();
        }

        // TODO: verify that Observers are Attached to all Steps
    }
}
