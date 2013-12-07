using System.Web.Mvc;
using Commerce.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Model;
using Commerce.Web.Plumbing;

namespace Commerce.UnitTests.Security
{
    [TestFixture]
    public class CommerceSecurityContextFactoryTest
    {
        [Test]
        public void AuthControllerIsPublic()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new UnsecuredController(null, null);
            var factory = new SecurityContextFactory();
            var user = new AggregateUser();
             
            // Act
            var result = factory.Create(context, user);

            // Assert
            Assert.AreEqual(AuthorizationZone.Public, result.AuthorizationZone);
            Assert.AreEqual(AccountLevel.NotApplicable, result.AccountLevelRestriction);
            Assert.AreEqual(false, result.IsPaymentArea);
            Assert.AreEqual(user, result.User);
        }

        [Test]
        public void ProductControllerIsAdminstrativeZone()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new Web.Controllers.ProductController(null, null, null, null, null, null, null);
            var factory = new SecurityContextFactory();
            var user = new AggregateUser();

            // Act
            var result = factory.Create(context, user);

            // Assert
            Assert.AreEqual(AuthorizationZone.Administrative, result.AuthorizationZone);
            Assert.AreEqual(AccountLevel.NotApplicable, result.AccountLevelRestriction);
            Assert.AreEqual(false, result.IsPaymentArea);
            Assert.AreEqual(user, result.User);
        }

        [Test]
        public void AdminHomeIsAdminstrativeZone()
        {
            //// Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new HomeController();
            var factory = new SecurityContextFactory();
            var user = new AggregateUser();

            // Act
            var result = factory.Create(context, user);

            // Assert
            Assert.AreEqual(AuthorizationZone.Administrative, result.AuthorizationZone);
            Assert.AreEqual(AccountLevel.NotApplicable, result.AccountLevelRestriction);
            Assert.AreEqual(false, result.IsPaymentArea);
            Assert.AreEqual(user, result.User);
        }
    }
}