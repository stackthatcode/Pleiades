using System;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Execution;

namespace Pleiades.Framework.UnitTests.Execution
{
    [TestFixture]
    public class TestFixtureForInsertedStepFactory
    {
        [Test]
        public void Verify_That_Factory_Inserts_Step_Before_And_Creates_Chosen_Step()
        {
            // Arrange
            var step1 = new StepXYZ();
            var step2 = new StepOk();
            var container = MockRepository.GenerateMock<IGenericContainer>();
            container.Expect(x => x.Resolve<StepXYZ>()).Return(step1);
            container.Expect(x => x.Resolve<StepOk>()).Return(step2);
            var factory = new InsertedStepXYZFactory(container);

            // Act
            StepComposite<ContextStub> factoryOutput = factory.Make<StepOk>();

            // Assert
            container.VerifyAllExpectations();
            Assert.AreEqual(2, factoryOutput.Steps.Count);
            Assert.AreSame(step1, factoryOutput.Steps[0]);
            Assert.AreSame(step2, factoryOutput.Steps[1]);
        }
    }

    public class InsertedStepXYZFactory : InsertedStepFactory<ContextStub, StepXYZ>
    {
        public InsertedStepXYZFactory(IGenericContainer container)
            : base(container)
        {
        }
    }
}