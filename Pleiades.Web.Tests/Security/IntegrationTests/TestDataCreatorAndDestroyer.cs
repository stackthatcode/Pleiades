using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;

namespace Pleiades.Web.Tests.Security.IntegrationTests
{
    [TestFixture]
    [Ignore]
    public class TestDataCreatorAndDestroyer
    {
        [Test]
        public void CreateUsers()
        {
            UserTestHelpers.ResetTestUser();
        }

        [Test]
        public void DestroyUsers()
        {
            UserTestHelpers.CleanupTestUser();
        }
    }
}
