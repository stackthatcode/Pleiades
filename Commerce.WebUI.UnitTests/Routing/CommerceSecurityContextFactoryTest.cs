using System;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Model;
using Commerce.Web.Areas.Admin.Controllers;
using Commerce.Web.Areas.Public.Controllers;
using Commerce.Web.Plumbing;

namespace Commerce.Web.UnitTests.Security
{
    [TestFixture]
    public class CommerceSecurityContextFactoryTest
    {
        [Test]
        public void AuthControllerIsPublic()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new AuthController(null, null);
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
        public void ProductControllerIsPublic()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new ProductsController();
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