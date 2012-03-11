﻿using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Gallio.Framework;
using MbUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Concrete;
using Pleiades.Utilities.TestHelpers.General;
using Pleiades.Utilities.TestHelpers.Web;

namespace Pleiades.Web.Tests.SecurityUnitTests
{
    [TestFixture]
    public class OwnerAuthorizationUnitTests
    {
        IOwnerAuthorizationService ownerauthservice = new OwnerAuthorizationService();

        [Test]
        public void OwnerCanAccessOwnResources()
        {
            OwnershipContext context = new OwnershipContext { OwnerDomainUserId = 333 };

            var user = SystemAuthorizationUnitTests.ActiveTrustedStandardUser;
            user.DomainUserId = 333;

            var result = ownerauthservice.Authorize(user, context);
            result.ShouldEqual(SecurityResponseCode.Allowed);
        }

        [Test]
        public void NonOwnerCannotAccessOwnResources()
        {
            OwnershipContext context = new OwnershipContext { OwnerDomainUserId = 333 };

            var user = SystemAuthorizationUnitTests.ActiveTrustedStandardUser;
            user.DomainUserId = 456;

            var result = ownerauthservice.Authorize(user, context);
            result.ShouldEqual(SecurityResponseCode.AccessDenied);
        }

        [Test]
        public void AdminCanAccessAnyResources()
        {
            OwnershipContext context = new OwnershipContext { OwnerDomainUserId = 333 };

            var user = SystemAuthorizationUnitTests.AdminUser;

            var result = ownerauthservice.Authorize(user, context);
            result.ShouldEqual(SecurityResponseCode.Allowed);
        }
    }
}
