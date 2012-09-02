using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Tests.Security.IntegrationTests
{
    [TestFixture]
    public class AggregateUserServiceTests
    {
        [Test]
        [ExpectedException(typeof(Exception))]
        public void Verify_Cant_Add_More_Than_Maximum_Number_Of_Admins()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var aggregateUserService = new  AggregateUserService(null, repository);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Admin)).Return(AggregateUserService.MaxAdminUsers);

            // Act
            PleiadesMembershipCreateStatus createStatus;
            aggregateUserService.Create(
                null, 
                new CreateOrModifyIdentityRequest() { UserRole = UserRole.Admin }, 
                out createStatus);

            // Assert - should throw!            
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Verify_Cant_Add_More_Than_Maximum_Number_Of_Supreme_Users()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var aggregateUserService = new AggregateUserService(null, repository);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Admin)).Return(1);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Supreme)).Return(AggregateUserService.MaxSupremeUsers);

            // Act
            PleiadesMembershipCreateStatus createStatus;
            aggregateUserService.Create(
                null,
                new CreateOrModifyIdentityRequest() { UserRole = UserRole.Supreme },
                out createStatus);

            // Assert - should throw!            
        }

        [Test]
        public void Verify_MembershipFailure_Create_Request()
        {
            // Arrange            
            var request = new CreateOrModifyIdentityRequest()
            {
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };

            // Arrange
            PleiadesMembershipCreateStatus createStatus;
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x
                .CreateUser(null, out createStatus))
                .IgnoreArguments()
                .OutRef(PleiadesMembershipCreateStatus.DuplicateUserName);

            var aggregateUserService = new AggregateUserService(membership, null);

            // Act
            var response =
                aggregateUserService.Create(
                    null,
                    new CreateOrModifyIdentityRequest() { UserRole = UserRole.Trusted },
                    out createStatus);

            // Assert
            membership.VerifyAllExpectations();
            Assert.AreNotEqual(PleiadesMembershipCreateStatus.Success, createStatus);
            Assert.IsNull(response);
        }

        [Test]
        public void Verify_Valid_Create_Request()
        {
            // Arrange            
            var request = new CreateOrModifyIdentityRequest()
            {
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };

            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository.Expect(x => x.Add(null)).IgnoreArguments();
            repository.Expect(x => x.SaveChanges());
            
            PleiadesMembershipCreateStatus createStatus;
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.CreateUser(null, out createStatus)).IgnoreArguments().OutRef(PleiadesMembershipCreateStatus.Success);

            var aggregateUserService = new AggregateUserService(membership, repository);

            // Act
            var response = 
                aggregateUserService.Create(
                    null,
                    new CreateOrModifyIdentityRequest() { UserRole = UserRole.Trusted },
                    out createStatus);

            // Assert
            repository.VerifyAllExpectations();
            membership.VerifyAllExpectations();
            Assert.IsNotNull(response);
        }

    }
}
