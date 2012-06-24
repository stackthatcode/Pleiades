using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Providers;

namespace Pleiades.Framework.UnitTests.Membership.Provider
{
    [TestFixture]
    public class MembershipProviderTests
    {
        PfMembershipProvider Provider;

        [SetUp]
        public void TestSetup()
        {
            Provider = new PfMembershipProvider();
            Provider.MembershipRepository = MockRepository.GenerateStub<IMembershipRepository>();
            var settings = MockRepository.GenerateStub<MembershipProviderSettings>();
            PfMembershipProvider.MembershipProviderSettings = settings;
        }

        [Test]
        public void CreateUser() 
        {

            MembershipCreateStatus statusOutput;
            var user = Provider.CreateUser(
                "12345789", "password1", "aleks@test.com", "Whawt's This?", "What it is", true, Guid.NewGuid(), out statusOutput);
        }
    }
}