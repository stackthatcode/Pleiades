using System;
using System.Collections.Generic;
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
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.IsValidPassword(password)).Return(false);
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
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.IsValidPassword(null)).Return(true).IgnoreArguments();

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
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.IsValidPassword(null)).Return(true).IgnoreArguments();

            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            var emailAddress = "jones@jones.com";
            repository.Expect(x => x.GetUserByEmail(emailAddress))
                        .IgnoreArguments()
                        .Return(null);
            
            repository.Expect(x => x.GetUserByUserName(username)).IgnoreArguments().Return(new PfMembershipUser());

            IPfMembershipService service = new PfMembershipService(repository, passwordService, null);

            var request = new CreateNewMembershipUserRequest
                {
                    UserName = username
                };

            // Act
            PfMembershipCreateStatus createStatus;
            var result = service.CreateUser(request, out createStatus);

            // Assert
            Assert.That(createStatus, Is.EqualTo(PfMembershipCreateStatus.DuplicateUserName));
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Create_User_Success_Test()
        {
            var username = "123456";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.IsValidPassword(null))
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
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_For_Approved_But_Locked_Out_Users()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser() { IsApproved = true, IsLockedOut = true});

            IPfMembershipService service = new PfMembershipService(repository, null, null);
                
            // Act
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_For_Unapproved_Users()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser() { IsApproved = false });

            IPfMembershipService service = new PfMembershipService(repository, null, null);

            // Act
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_For_InvalidPassword()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            
            var storedPasswordHash = "1234567890ABCDEF";
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser() { IsApproved = true, Password = storedPasswordHash });

            var passwordAttempt = "123456789";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckSecureInformation(passwordAttempt, storedPasswordHash)).Return(false);

            IPfMembershipService service = new PfMembershipService(repository, passwordService, null);

            // Act
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", passwordAttempt);

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_User_With_ValidPassword()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();

            var storedPasswordHash = "1234567890ABCDEF";
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser() { IsApproved = true, Password = storedPasswordHash });

            var passwordAttempt = "123456789";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckSecureInformation(passwordAttempt, storedPasswordHash)).Return(true);

            IPfMembershipService service = new PfMembershipService(repository, passwordService, null);
                
            // Act            
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", passwordAttempt);

            // Assert
            Assert.That(result, Is.Not.Null);
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
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

        [Test]
        public void GetAllUsers_Correctly_Invokes_Repository()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetAllUsers()).Return(new List<PfMembershipUser>());

            IPfMembershipService service = new PfMembershipService(repository, null, null);

            // Act
            var result = service.GetAllUsers();

            // Assert
            repository.VerifyAllExpectations();            
        }
        
        [Test]
        public void GetNumberOfUsersOnline()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            var onlineWindow = new TimeSpan();
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.UserIsOnlineTimeWindow).Return(onlineWindow);
            repository.Expect(x => x.GetNumberOfUsersOnline(onlineWindow)).Return(22);
            IPfMembershipService service = new PfMembershipService(repository, null, settings);

            // Act
            var result = service.GetNumberOfUsersOnline();

            // Assert
            Assert.That(result, Is.EqualTo(22));
            repository.VerifyAllExpectations(); 
            settings.VerifyAllExpectations();           
        }

        [Test]
        public void UnlockUser_Gets_User_From_Repository_And_Changes_IsLocked_ToFalse()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            var user = new PfMembershipUser() { UserName = "12345", IsLockedOut = true};
            repository.Expect(x => x.GetUserByUserName("12345")).Return(user);

            // Act
            IPfMembershipService service = new PfMembershipService(repository, null, null);
            service.UnlockUser("12345");

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(user.IsLockedOut, Is.False);
        }

        [Test]
        public void PasswordQuestion_Correctly_Invokes_Repository()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByUserName("123456")).Return(new PfMembershipUser { PasswordQuestion = "Who lives on Mars?" });
            IPfMembershipService service = new PfMembershipService(repository, null, null);

            // Act
            var result = service.PasswordQuestion("123456");

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo("Who lives on Mars?"));
        }
        
        [Test]
        public void ResetPassword_Fails_If_User_Doesnt_Exist()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByUserName("12345")).Return(null);

            // Act
            IPfMembershipService service = new PfMembershipService(repository, null, null);
            PfPasswordChangeStatus status;
            var result = service.ResetPassword("12345", null, false, out status);
            
            // Assert
            repository.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfPasswordChangeStatus.UserDoesNotExist));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Fails_If_User_Is_Unapproved()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository.Expect(x => x.GetUserByUserName("12345")).Return(new PfMembershipUser { IsApproved = false});

            // Act
            IPfMembershipService service = new PfMembershipService(repository, null, null);
            PfPasswordChangeStatus status;
            var result = service.ResetPassword("12345", null, false, out status);

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfPasswordChangeStatus.UserIsNotApproved));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Fails_If_User_Is_LockedOut()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(new PfMembershipUser { IsApproved = true, IsLockedOut = true});

            // Act
            IPfMembershipService service = new PfMembershipService(repository, null, null);
            PfPasswordChangeStatus status;
            var result = service.ResetPassword("12345", null, false, out status);

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfPasswordChangeStatus.UserIsLockedOut));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Fails_If_PasswordReset_Disabled()
        {
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(new PfMembershipUser { IsApproved = true, IsLockedOut = false });

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            settings.Expect(x => x.EnablePasswordReset).Return(false);

            // Act
            IPfMembershipService service = new PfMembershipService(repository, null, settings);
            PfPasswordChangeStatus status;
            var result = service.ResetPassword("12345", null, false, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfPasswordChangeStatus.PasswordResetIsNotEnabled));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Fails_If_Answer_Required_But_Not_Supplied()
        {
            var user = new PfMembershipUser {IsApproved = true, IsLockedOut = false};
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            settings.Expect(x => x.EnablePasswordReset).Return(true);
            settings.Expect(x => x.RequiresQuestionAndAnswer).Return(true);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.UpdateFailedQuestionAndAnswerAttempts(user));

            // Act
            IPfMembershipService service = new PfMembershipService(repository, passwordService, settings);
            PfPasswordChangeStatus status;
            var result = service.ResetPassword("12345", null, false, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfPasswordChangeStatus.AnswerRequiredForPasswordReset));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Fails_If_Wrong_Answer_Given_For_Security_Question()
        {
            var user = new PfMembershipUser { IsApproved = true, IsLockedOut = false};
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            settings.Expect(x => x.EnablePasswordReset).Return(true);
            settings.Expect(x => x.RequiresQuestionAndAnswer).Return(true);
            
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckSecureInformation("adsfsdkfl;", "1235kl;k;34")).Return(false);
            passwordService.Expect(x => x.UpdateFailedQuestionAndAnswerAttempts(user));

            // Act
            IPfMembershipService service = new PfMembershipService(repository, passwordService, settings);
            PfPasswordChangeStatus status;
            var result = service.ResetPassword("12345", "adsfsdkfl;", false, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfPasswordChangeStatus.WrongAnswerSuppliedForSecurityQuestion));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Verify_That_AdminOverride_Ignores_()
        {
            var encodedPasswordAnswer = "7ABDE1234ABE";
            var user = new PfMembershipUser { IsApproved = true, IsLockedOut = false, PasswordAnswer = encodedPasswordAnswer };
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            
            var newPassword = "12sdgjkl325";
            var encodedPassword = "ABVBKLEEW";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.GeneratePassword()).Return(newPassword);
            passwordService.Expect(x => x.EncodePassword(newPassword)).Return(encodedPassword);

            // Act
            IPfMembershipService service = new PfMembershipService(repository, passwordService, settings);
            PfPasswordChangeStatus status;
            var result = service.ResetPassword("12345", "adsfsdkfl;", true, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfPasswordChangeStatus.Success));
            Assert.That(result, Is.EqualTo(newPassword));
            Assert.That(user.Password, Is.EqualTo(encodedPassword));
        }

        [Test]
        public void ResetPassword_Success_Path()
        {
            var encodedPasswordAnswer = "7ABDE1234ABE";
            var user = new PfMembershipUser { IsApproved = true, IsLockedOut = false, PasswordAnswer = encodedPasswordAnswer };
            var repository = MockRepository.GenerateMock<IMembershipRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            settings.Expect(x => x.EnablePasswordReset).Return(true);
            settings.Expect(x => x.RequiresQuestionAndAnswer).Return(true);

            var newPassword = "12sdgjkl325";
            var encodedPassword = "ABVBKLEEW";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckSecureInformation("adsfsdkfl;", encodedPasswordAnswer)).Return(true);
            passwordService.Expect(x => x.GeneratePassword()).Return(newPassword);
            passwordService.Expect(x => x.EncodePassword(newPassword)).Return(encodedPassword);

            // Act
            IPfMembershipService service = new PfMembershipService(repository, passwordService, settings);
            PfPasswordChangeStatus status;
            var result = service.ResetPassword("12345", "adsfsdkfl;", false, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfPasswordChangeStatus.Success));
            Assert.That(result, Is.EqualTo(newPassword));
            Assert.That(user.Password, Is.EqualTo(encodedPassword));
        }

    }
}
