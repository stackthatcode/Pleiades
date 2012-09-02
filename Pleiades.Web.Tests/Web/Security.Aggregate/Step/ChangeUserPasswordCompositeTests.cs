using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Composites;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    [TestFixture]
    public class ChangeUserPasswordCompositeTests
    {
        [Test]
        public void Verify_That_OwnerAuthorizationCompositeBase_Inserts_Step_Before_And_Creates_Chosen_Step()
        {
            // Arrange
            var container = MockRepository.GenerateMock<IGenericContainer>();
            var step0 = new SimpleOwnerAuthorizationStep<ChangeUserPasswordContext>();
            var step1 = new ChangeUserPasswordStep(null);
            container.Expect(x => 
                x.Resolve<SimpleOwnerAuthorizationStep<ChangeUserPasswordContext>>()).Return(step0);

            // Act
            var composite = new ChangeUserPasswordComposite(container, step1);

            // Assert
            container.VerifyAllExpectations();
            Assert.AreEqual(2, composite.Steps.Count);
            Assert.AreSame(step0, composite.Steps[0]);
            Assert.AreSame(step1, composite.Steps[1]);
        }
    }
}