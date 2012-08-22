using System;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.Identity.Model;
using Pleiades.Commerce.WebUI.Areas.Admin.Controllers;
using Pleiades.Commerce.WebUI.Areas.Public.Controllers;
using Pleiades.Commerce.WebUI.Plumbing.Security;

namespace Pleiades.Commerce.WebUI.UnitTests.Security
{
    [TestFixture]
    public class TestsForMySecurityContextBuilder
    {
        [Test]
        public void LoginControllerIsPublic()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.Controller = new LoginController(null);
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