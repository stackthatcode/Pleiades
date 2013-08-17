using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.UnitTests.Concrete
{
    [TestFixture]
    public class PfMembershipServiceTests
    {
        [Test]
        public void Create_User_With_Invalid_Password_Should_Fail()
        {
            var password = "123456";
            var passwordService = MockRepository.GenerateMock<IPasswordServices>();
            passwordService.Expect(x => x.IsValid(password)).Return(false);
            IPfMembershipService service = new PfMembershipService(null, passwordService, null);

            var request = new CreateNewMembershipUserRequest
                {
                    Password = password
                };

            PfMembershipCreateStatus createStatus;

            var result = service.CreateUser(request, out createStatus);

            Assert.That(createStatus, Is.EqualTo(PfMembershipCreateStatus.InvalidPassword));
            Assert.That(result, Is.Null);
            passwordService.VerifyAllExpectations();
        }

        [Test]
        public void Create_User_With_Duplicate_EmailAddress_Should_Fail()
        {
            var passwordService = MockRepository.GenerateMock<IPasswordServices>();
            passwordService.Expect(x => x.IsValid(null)).Return(true).IgnoreArguments();

            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            
            var emailAddress = "jones@jones.com";
            repository.Expect(x => x.GetUserByEmail(emailAddress))
                      .Return(new PfMembershipUser());

            IPfMembershipService service = new PfMembershipService(repository, passwordService, null);

            var request = new CreateNewMembershipUserRequest
            {
                Email = emailAddress
            };

            // Act
            PfMembershipCreateStatus createStatus;
            var result = service.CreateUser(request, out createStatus);

            // Assert
            Assert.That(createStatus, Is.EqualTo(PfMembershipCreateStatus.DuplicateEmail));
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Create_User_With_Duplicate_UserName_Should_Fail()
        {
            var username = "123456";
            var passwordService = MockRepository.GenerateMock<IPasswordServices>();
            passwordService.Expect(x => x.IsValid(null)).Return(true).IgnoreArguments();

            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            var emailAddress = "jones@jones.com";
            repository.Expect(x => x.GetUserByEmail(emailAddress))
                        .IgnoreArguments()
                        .Return(null);
            
            repository.Expect(x => x.GetUserByUserName(username)).IgnoreArguments().Return( new PfMembershipUser());
            
            IPfMembershipService service = new PfMembershipService(repository, passwordService, null);

            var request = new CreateNewMembershipUserRequest
                {
                    UserName = username
                };

            PfMembershipCreateStatus createStatus;

            var result = service.CreateUser(request, out createStatus);

            Assert.That(createStatus, Is.EqualTo(PfMembershipCreateStatus.DuplicateUserName));
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Create_User_Success_Test()
        {
            var username = "123456";
            var passwordService = MockRepository.GenerateMock<IPasswordServices>();
            passwordService.Expect(x => x.IsValid(null))
                        .Return(true)
                        .IgnoreArguments();

            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            var emailAddress = "jones@jones.com";
            repository.Expect(x => x.GetUserByEmail(emailAddress))
                        .IgnoreArguments()
                        .Return(null);

            repository.Expect(x => x.GetUserByUserName(username))
                        .IgnoreArguments()
                        .Return(null);

            IPfMembershipService service = new PfMembershipService(repository, passwordService, null);
            var request = new CreateNewMembershipUserRequest
            {
                UserName = username
            };

            // Act            
            PfMembershipCreateStatus createStatus;
            var result = service.CreateUser(request, out createStatus);

            // Assert
            Assert.That(createStatus, Is.EqualTo(PfMembershipCreateStatus.Success));
            Assert.That(result, Is.Not.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException()]
        public void GenerateUniqueUserName_Keeps_Trying_Until_MaxAttempts()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByUserName(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser())
                      .Repeat.Times(3);

            IPfMembershipService service = new PfMembershipService(repository, null, null);

            // Act            
            PfMembershipCreateStatus createStatus;
            var result = service.GenerateUniqueUserName(3);

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void GenerateUniqueUserName_Returns_UserName_Before_MaxAttempts()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByUserName(null))
                      .IgnoreArguments()
                      .Return(null);
            
            IPfMembershipService service = new PfMembershipService(repository, null, null);

            // Act            
            PfMembershipCreateStatus createStatus;
            var result = service.GenerateUniqueUserName(10);

            // Assert
            Assert.That(result, Is.Not.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_For_Non_Existent_User()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(null);

            IPfMembershipService service = new PfMembershipService(repository, null, null);

            // Act            
            PfMembershipCreateStatus createStatus;
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_InvalidPassword()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser() { Password = "1235345ABEDF222"});

            var passwordAttempt = "123456789";
            var encryptionService = MockRepository.GenerateMock<IEncryptionService>();
            encryptionService.Expect(x => x.MakeHMAC256Base64(passwordAttempt)).Return("1234567890ABCDEF");

            IPfMembershipService service = new PfMembershipService(repository, null, encryptionService);

            // Act            
            PfMembershipCreateStatus createStatus;
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_User_With_ValidPassword()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser() { Password = "1235345ABEDF222" });

            var passwordAttempt = "123456789";
            var encryptionService = MockRepository.GenerateMock<IEncryptionService>();
            encryptionService.Expect(x => x.MakeHMAC256Base64(passwordAttempt)).Return("1235345ABEDF222");

            IPfMembershipService service = new PfMembershipService(repository, null, encryptionService);

            // Act            
            PfMembershipCreateStatus createStatus;
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void GetUserByUserName_Correctly_Invokes_Repository()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByUserName("123456")).Return(new PfMembershipUser());
            
            IPfMembershipService service = new PfMembershipService(repository, null, null);

            // Act
            var result = service.GetUserByUserName("123456");

            // Assert
            repository.VerifyAllExpectations();            
        }

        [Test]
        public void GetUserByEmail_Correctly_Invokes_Repository()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByEmail("ajones@dfgkgl.com")).Return(new PfMembershipUser());

            IPfMembershipService service = new PfMembershipService(repository, null, null);

            // Act
            var result = service.GetUserByEmail("ajones@dfgkgl.com");

            // Assert
            repository.VerifyAllExpectations();
        }


    }
}
