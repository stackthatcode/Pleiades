using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Commerce.IntegrationTests.Security
{
    [TestFixture]
    public class AggregateUserRepositoryTests
    {
        [Test]
        public void TestThatRepo_And_Verify()
        {
            // TODO...
        }

        //[Test]
        //public void Verify_RetrieveUserById()
        //{
        //    // Arrange       
        //    var repository = MockRepository.GenerateMock<IIdentityRepository>();
        //    var identityService = new IdentityUserService(repository);
        //    var testUser = new IdentityProfile() { ID = 123 };
        //    repository.Expect(x => x.RetrieveUserById(123)).Return(testUser);

        //    // Act
        //    var user = identityService.RetrieveUserById(123);

        //    // Assert
        //    repository.VerifyAllExpectations();
        //    Assert.AreEqual(123, user.ID);
        //}

        //[Test]
        //public void Verify_RetrieveTotalUsers()
        //{
        //    // Arrange       
        //    var repository = MockRepository.GenerateMock<IIdentityRepository>();
        //    var identityService = new IdentityUserService(repository);
        //    repository.Expect(x => x.Count()).Return(9300);

        //    // Act
        //    var count = identityService.RetrieveTotalUsers();

        //    // Assert
        //    repository.VerifyAllExpectations();
        //    Assert.AreEqual(9300, count);
        //}

        //[Test]
        //public void Verify_UpdateMethod()
        //{
        //    // Arrange       
        //    var repository = MockRepository.GenerateMock<IIdentityRepository>();
        //    var testUser = new IdentityProfile() { ID = 123 };
        //    repository.Expect(x => x.RetrieveUserById(123)).Return(testUser);
        //    repository.Expect(x => x.SaveChanges());
        //    var identityService = new IdentityUserService(repository);

        //    // Act
        //    identityService.Update(new CreateOrModifyIdentityRequest() { ID = 123 });

        //    // Assert
        //    repository.VerifyAllExpectations();
        //}

        //[Test]
        //public void RetrieveUserByEmailAddress()
        //{
        //    var service = new DomainUserService();
        //    var user = service.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);
        //    Assert.IsNotNull(user);
        //}

        //[Test]
        //public void RetrieveUserByMembershipUserName()
        //{
        //    var service = new DomainUserService();
        //    var user = service.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);
        //    Assert.IsNotNull(user);

        //    var user2 = service.RetrieveUserByMembershipUserName(user.MembershipUser.UserName);
        //    Assert.IsNotNull(user2);
        //}
    }
}
