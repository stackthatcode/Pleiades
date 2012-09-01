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
    public class IdentityUserServiceTests
    {
        [Test]
        public void GetUserCount_SimpleTest()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var identityService = new IdentityUserService(repository);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Trusted)).Return(3);

            // Act
            var count = identityService.GetUserCountByRole(UserRole.Trusted);

            // Assert
            Assert.AreEqual(3, count);
            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Verify_Cant_Add_More_Than_Maximum_Number_Of_Admins()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var identityService = new IdentityUserService(repository);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Admin)).Return(IdentityUserService.MaxAdminUsers);

            // Act
            identityService.Create(new CreateOrModifyIdentityUserRequest()
            {
                 UserRole = UserRole.Admin
            });

            // Assert - should throw!            
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Verify_Cant_Add_More_Than_Maximum_Number_Of_Supreme_Users()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var identityService = new IdentityUserService(repository);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Supreme)).Return(IdentityUserService.MaxSupremeUsers);

            // Act
            identityService.Create(new CreateOrModifyIdentityUserRequest()
            {
                UserRole = UserRole.Supreme
            });

            // Assert - should throw!            
        }

        [Test]
        public void Verify_Valid_Create_Request()
        {
            // Arrange            
            var request = new CreateOrModifyIdentityUserRequest()
            {
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };

            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var identityService = new IdentityUserService(repository);
            repository.Expect(x => x.Add(null)).IgnoreArguments();
            repository.Expect(x => x.SaveChanges());

            // Act
            identityService.Create(request);

            // Assert
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Verify_RetrieveUserById()
        {
            // Arrange       
            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var identityService = new IdentityUserService(repository);
            var testUser = new IdentityUser() { ID = 123 };
            repository.Expect(x => x.RetrieveUserById(123)).Return(testUser);

            // Act
            var user = identityService.RetrieveUserById(123);
            
            // Assert
            repository.VerifyAllExpectations();
            Assert.AreEqual(123, user.ID);
        }

        [Test]
        public void Verify_RetrieveTotalUsers()
        {
            // Arrange       
            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var identityService = new IdentityUserService(repository);
            repository.Expect(x => x.Count()).Return(9300);
            
            // Act
            var count = identityService.RetrieveTotalUsers();
            
            // Assert
            repository.VerifyAllExpectations();
            Assert.AreEqual(9300, count);
        }

        [Test]
        public void Verify_UpdateMethod()
        {
            // Arrange       
            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var testUser = new IdentityUser() { ID = 123 };
            repository.Expect(x => x.RetrieveUserById(123)).Return(testUser);
            repository.Expect(x => x.SaveChanges());
            var identityService = new IdentityUserService(repository);

            // Act
            identityService.Update(new CreateOrModifyIdentityUserRequest() { ID = 123 });

            // Assert
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Verify_UpdateLastModified()
        {
            // Arrange       
            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var testUser = new IdentityUser() { ID = 123 };
            repository.Expect(x => x.RetrieveUserById(123)).Return(testUser);
            repository.Expect(x => x.SaveChanges());
            var identityService = new IdentityUserService(repository);

            // Act
            identityService.UpdateLastModified(123);
                
            // Assert
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Delete_Method()
        {
            // Arrange       
            var repository = MockRepository.GenerateMock<IIdentityRepository>();
            var testUser = new IdentityUser() { ID = 123 };
            repository.Expect(x => x.RetrieveUserById(123)).Return(testUser);
            repository.Expect(x => x.Delete(testUser));
            repository.Expect(x => x.SaveChanges());
            var identityService = new IdentityUserService(repository);

            // Act
            identityService.Delete(123);
            
            // Assert
            repository.VerifyAllExpectations();
        }
    }
}
