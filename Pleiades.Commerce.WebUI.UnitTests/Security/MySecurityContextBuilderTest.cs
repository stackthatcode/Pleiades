using System;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Model;
using Commerce.WebUI.Areas.Admin.Controllers;
using Commerce.WebUI.Areas.Public.Controllers;
using Commerce.WebUI.Plumbing.Security;

namespace Commerce.WebUI.UnitTests.Security
{
    [TestFixture]
    public class MySecurityContextBuilderTest
    {
        [Test]
        public void LoginControllerIsPublic()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new AuthController(null);
            var builder = new MySystemAuthContextBuilder();

            // Act
            var result = builder.Build(context);

            // Assert
            Assert.AreEqual(AuthorizationZone.Public, result.AuthorizationZone);
            Assert.AreEqual(AccountLevel.NotApplicable, result.AccountLevelRestriction);
            Assert.AreEqual(false, result.IsPaymentArea);
        }

        [Test]
        public void ProductControllerIsPublic()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new ProductsController();
            var builder = new MySystemAuthContextBuilder();

            // Act
            var result = builder.Build(context);

            // Assert
            Assert.AreEqual(AuthorizationZone.Public, result.AuthorizationZone);
            Assert.AreEqual(AccountLevel.NotApplicable, result.AccountLevelRestriction);
            Assert.AreEqual(false, result.IsPaymentArea);
        }

        [Test]
        public void AdminHomeIsSecured()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new HomeController();
            var builder = new MySystemAuthContextBuilder();

            // Act
            var result = builder.Build(context);

            // Assert
            Assert.AreEqual(AuthorizationZone.Administrative, result.AuthorizationZone);
            Assert.AreEqual(AccountLevel.NotApplicable, result.AccountLevelRestriction);
            Assert.AreEqual(false, result.IsPaymentArea);
        }
    }
}