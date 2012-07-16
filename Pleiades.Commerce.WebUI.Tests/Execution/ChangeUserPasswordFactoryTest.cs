using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Commerce.Web.Security.Execution.NonPublic;
using Pleiades.Commerce.Web.Security.Factories;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.Injection;

namespace Pleiades.Commerce.Web.UnitTests.Execution
{
    [TestFixture]
    public class ChangeUserPasswordFactoryTest
    {
        [Test]
        public void Verify_That_Factory_Inserts_Step_Before_And_Creates_Chosen_Step()
        {
            // Arrange
            var step1 = new SimpleOwnerAuthorizationStep<ChangeUserPasswordContext>();
            var step2 = new ChangeUserPasswordStep(null);

            var container = MockRepository.GenerateMock<IGenericContainer>();
            container.Expect(x => x.Resolve<SimpleOwnerAuthorizationStep<ChangeUserPasswordContext>>()).Return(step1);
            container.Expect(x => x.Resolve<ChangeUserPasswordStep>()).Return(step2);
            var factory = new ChangeUserPasswordStepFactory(container);

            // Act
            StepComposite<ChangeUserPasswordContext> factoryOutput = factory.Make<ChangeUserPasswordStep>();

            // Assert
            container.VerifyAllExpectations();
            Assert.AreEqual(2, factoryOutput.Steps.Count);
            Assert.AreSame(step1, factoryOutput.Steps[0]);
            Assert.AreSame(step2, factoryOutput.Steps[1]);
        }
    }
}
