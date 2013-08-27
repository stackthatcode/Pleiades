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
            IPfMembershipService service = new PfMembershipService(null, null, passwordService, null);

            var request = new PfCreateNewMembershipUserRequest
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

            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            
            var emailAddress = "jones@jones.com";
            repository.Expect(x => x.GetUserByEmail(emailAddress))
                      .Return(new PfMembershipUser());

            IPfMembershipService service = new PfMembershipService(repository, null, passwordService, null);

            var request = new PfCreateNewMembershipUserRequest
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

            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            var emailAddress = "jones@jones.com";
            repository.Expect(x => x.GetUserByEmail(emailAddress))
                        .IgnoreArguments()
                        .Return(null);
            
            repository.Expect(x => x.GetUserByUserName(username)).IgnoreArguments().Return(new PfMembershipUser());

            IPfMembershipService service = new PfMembershipService(repository, null, passwordService, null);

            var request = new PfCreateNewMembershipUserRequest
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

            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            var emailAddress = "jones@jones.com";
            repository.Expect(x => x.GetUserByEmail(emailAddress))
                        .IgnoreArguments()
                        .Return(null);

            repository.Expect(x => x.GetUserByUserName(username))
                        .IgnoreArguments()
                        .Return(null);

            var writable_repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            writable_repository.Expect(x => x.AddUser(null)).IgnoreArguments();
            
            // TODO: verify user identity, etc.

            IPfMembershipService service = new PfMembershipService(repository, writable_repository, passwordService, null);
            var request = new PfCreateNewMembershipUserRequest
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
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository.Expect(x => x.GetUserByUserName(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser())
                      .Repeat.Times(3);

            IPfMembershipService service = new PfMembershipService(repository, null, null, null);

            // Act            
            var result = service.GenerateUniqueUserName(3);

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void GenerateUniqueUserName_Returns_UserName_Before_MaxAttempts()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository.Expect(x => x.GetUserByUserName(null))
                      .IgnoreArguments()
                      .Return(null);

            IPfMembershipService service = new PfMembershipService(repository, null, null, null);

            // Act
            var result = service.GenerateUniqueUserName(10);

            // Assert
            Assert.That(result, Is.Not.Null);
            repository.VerifyAllExpectations();
        }        


        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_For_Non_Existent_User()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(null);

            IPfMembershipService service = new PfMembershipService(repository, null, null, null);

            // Act
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_For_Approved_But_Locked_Out_Users()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser() { IsApproved = true, IsLockedOut = true});

            IPfMembershipService service = new PfMembershipService(repository, null, null, null);
                
            // Act
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_For_Unapproved_Users()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(new PfMembershipUser() { IsApproved = false });

            IPfMembershipService service = new PfMembershipService(repository, null, null, null);

            // Act
            var result = service.ValidateUserByEmailAddr("aleks@aleks.com", "1233455");

            // Assert
            Assert.That(result, Is.Null);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void ValidateUserByEmailAddr_Returns_Null_For_InvalidPassword()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            
            var storedPasswordHash = "1234567890ABCDEF";
            var user = new PfMembershipUser() {IsApproved = true, Password = storedPasswordHash};
            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(user);

            var passwordAttempt = "123456789";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword(passwordAttempt, user)).Return(false);

            IPfMembershipService service = new PfMembershipService(repository, null, passwordService, null);

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
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            var storedPasswordHash = "1234567890ABCDEF";
            var user = new PfMembershipUser() { IsApproved = true, Password = storedPasswordHash };

            repository.Expect(x => x.GetUserByEmail(null))
                      .IgnoreArguments()
                      .Return(user);

            var passwordAttempt = "123456789";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword(passwordAttempt, user)).Return(true);

            IPfMembershipService service = new PfMembershipService(repository, null, passwordService, null);
                
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
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository.Expect(x => x.GetUserByUserName("123456")).Return(new PfMembershipUser());

            IPfMembershipService service = new PfMembershipService(repository, null, null, null);
                
            // Act
            var result = service.GetUserByUserName("123456");

            // Assert
            repository.VerifyAllExpectations();            
        }

        [Test]
        public void GetUserByEmail_Correctly_Invokes_Repository()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository.Expect(x => x.GetUserByEmail("ajones@dfgkgl.com")).Return(new PfMembershipUser());

            IPfMembershipService service = new PfMembershipService(repository, null, null, null);

            // Act
            var result = service.GetUserByEmail("ajones@dfgkgl.com");

            // Assert
            repository.VerifyAllExpectations();
        }

        [Test]
        public void GetAllUsers_Correctly_Invokes_Repository()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository.Expect(x => x.GetAllUsers()).Return(new List<PfMembershipUser>());

            IPfMembershipService service = new PfMembershipService(repository, null, null, null);

            // Act
            var result = service.GetAllUsers();

            // Assert
            repository.VerifyAllExpectations();            
        }
        
        [Test]
        public void GetNumberOfUsersOnline()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            var onlineWindow = new TimeSpan();
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.UserIsOnlineTimeWindow).Return(onlineWindow);
            repository.Expect(x => x.GetNumberOfUsersOnline(onlineWindow)).Return(22);
            IPfMembershipService service = new PfMembershipService(repository, null, null, settings);

            // Act
            var result = service.GetNumberOfUsersOnline();

            // Assert
            Assert.That(result, Is.EqualTo(22));
            repository.VerifyAllExpectations(); 
            settings.VerifyAllExpectations();           
        }


        [Test]
        public void PasswordQuestion_Correctly_Invokes_Repository()
        {
            var repository = MockRepository.GenerateMock<IMembershipReadOnlyRepository>();
            repository
                .Expect(x => x.GetUserByUserName("123456"))
                .Return(new PfMembershipUser { PasswordQuestion = "Who lives on Mars?" });
            IPfMembershipService service = new PfMembershipService(repository, null, null, null);

            // Act
            var result = service.PasswordQuestion("123456");

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo("Who lives on Mars?"));
        }


        [Test]
        public void UnlockUser_Gets_User_From_Repository_And_Changes_IsLocked_ToFalse()
        {
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            var user = new PfMembershipUser() { UserName = "12345", IsLockedOut = true };
            repository.Expect(x => x.GetUserByUserName("12345")).Return(user);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            service.UnlockUser("12345");

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(user.IsLockedOut, Is.False);
        }


        [Test]
        public void ResetPassword_Fails_If_User_Not_Active()
        {
            var user = MockRepository.GenerateMock<PfMembershipUser>();
            user.Expect(x => x.ActiveUser).Return(false);

            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);
            
            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            PfCredentialsChangeStatus status;
            var result = service.ResetPassword("12345", null, false, out status);

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfCredentialsChangeStatus.InactiveUser));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Fails_If_PasswordReset_Disabled()
        {
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(new PfMembershipUser { IsApproved = true, IsLockedOut = false });

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            settings.Expect(x => x.EnablePasswordReset).Return(false);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, settings);
            PfCredentialsChangeStatus status;
            var result = service.ResetPassword("12345", null, false, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfCredentialsChangeStatus.PasswordResetIsNotEnabled));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Fails_If_Answer_Required_But_Not_Supplied()
        {
            var user = new PfMembershipUser {IsApproved = true, IsLockedOut = false};
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            settings.Expect(x => x.EnablePasswordReset).Return(true);
            settings.Expect(x => x.RequiresQuestionAndAnswer).Return(true);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.UpdateFailedQuestionAndAnswerAttempts(user));

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, settings);
            PfCredentialsChangeStatus status;
            var result = service.ResetPassword("12345", null, false, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfCredentialsChangeStatus.AnswerRequiredForPasswordReset));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Fails_If_Wrong_Answer_Given_For_Security_Question()
        {
            var user = new PfMembershipUser { IsApproved = true, IsLockedOut = false};
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            settings.Expect(x => x.EnablePasswordReset).Return(true);
            settings.Expect(x => x.RequiresQuestionAndAnswer).Return(true);
            
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword("adsfsdkfl;", user)).Return(false);
            passwordService.Expect(x => x.UpdateFailedQuestionAndAnswerAttempts(user));

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, settings);
            PfCredentialsChangeStatus status;
            var result = service.ResetPassword("12345", "adsfsdkfl;", false, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfCredentialsChangeStatus.WrongAnswerSupplied));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ResetPassword_Verify_That_AdminOverride_Ignores_Settings_And_Wrong_Password()
        {
            var encodedPasswordAnswer = "7ABDE1234ABE";
            var user = new PfMembershipUser { IsApproved = true, IsLockedOut = false, PasswordAnswer = encodedPasswordAnswer };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            
            var newPassword = "12sdgjkl325";
            var encodedPassword = "ABVBKLEEW";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.GeneratePassword()).Return(newPassword);
            passwordService.Expect(x => x.EncodeSecureInformation(newPassword)).Return(encodedPassword);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, settings);
            PfCredentialsChangeStatus status;
            var result = service.ResetPassword("12345", "adsfsdkfl;", true, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfCredentialsChangeStatus.Success));
            Assert.That(result, Is.EqualTo(newPassword));
            Assert.That(user.Password, Is.EqualTo(encodedPassword));
        }

        [Test]
        public void ResetPassword_Success_Path()
        {
            var encodedPasswordAnswer = "7ABDE1234ABE";
            var user = new PfMembershipUser { IsApproved = true, IsLockedOut = false, PasswordAnswer = encodedPasswordAnswer };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var settings = MockRepository.GenerateMock<IPfMembershipSettings>();
            settings.Expect(x => x.EnablePasswordReset).Return(true);
            settings.Expect(x => x.RequiresQuestionAndAnswer).Return(true);

            var newPassword = "12sdgjkl325";
            var encodedPassword = "ABVBKLEEW";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPasswordAnswer("adsfsdkfl;", user)).Return(true);
            passwordService.Expect(x => x.GeneratePassword()).Return(newPassword);
            passwordService.Expect(x => x.EncodeSecureInformation(newPassword)).Return(encodedPassword);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, settings);
            PfCredentialsChangeStatus status;
            var result = service.ResetPassword("12345", "adsfsdkfl;", false, out status);

            // Assert
            repository.VerifyAllExpectations();
            settings.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(status, Is.EqualTo(PfCredentialsChangeStatus.Success));
            Assert.That(result, Is.EqualTo(newPassword));
            Assert.That(user.Password, Is.EqualTo(encodedPassword));
        }


        [Test]
        public void ChangePassword_Verify_That_AdminOverride_Ignores_Wrong_Password()
        {
            var encodedPasswordAnswer = "7ABDE1234ABE";
            var user = new PfMembershipUser { IsApproved = true, IsLockedOut = false, PasswordAnswer = encodedPasswordAnswer };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var newPassword = "12sdgjkl325";
            var encodedPassword = "ABVBKLEEW";
            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.EncodeSecureInformation(newPassword)).Return(encodedPassword);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangePassword("12345", "adsfsdkfl;", newPassword, true);
            
            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.Success));
            Assert.That(user.Password, Is.EqualTo(encodedPassword));
        }

        [Test]
        public void ChangePassword_Fails_If_User_Not_Active()
        {
            var wrongPassword = "wrongo!!!!";
            var newPassword = "12sdgjkl325";

            var user = MockRepository.GenerateMock<PfMembershipUser>();
            user.Expect(x => x.ActiveUser).Return(false);

            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            var result = service.ChangePassword("12345", wrongPassword, newPassword, false);

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.InactiveUser));
        }

        [Test]
        public void ChangePassword_Fails_If_Wrong_Password_Submitted()
        {
            var wrongPassword = "wrongo!!!!";
            var oldEncodedPassword = "adsfsdkfl;";
            var newPassword = "12sdgjkl325";

            var user = new PfMembershipUser
                {
                    IsApproved = true, IsLockedOut = false, Password = oldEncodedPassword
                };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword(wrongPassword, user)).Return(false);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangePassword("12345", wrongPassword, newPassword, false);

            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.WrongPasswordSupplied));
            Assert.That(user.Password, Is.EqualTo(oldEncodedPassword));
        }

        [Test]
        public void ChangePassword_HappyPath()
        {
            var oldPassword = "oldPassword123";
            var oldEncodedPassword = "abcd354b23b5==";
            var newPassword = "12sdgjkl325";
            var newEncodedPassword = "346bcd354b23b5==";

            var user = new PfMembershipUser
            {
                IsApproved = true,
                IsLockedOut = false,
                Password = oldEncodedPassword
            };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword(oldPassword, user)).Return(true);
            passwordService.Expect(x => x.EncodeSecureInformation(newPassword)).Return(newEncodedPassword);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangePassword("12345", oldPassword, newPassword, false);

            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.Success));
            Assert.That(user.Password, Is.EqualTo(newEncodedPassword));
        }


        [Test]
        public void ChangePasswordQuestionAndAnswer_Fails_With_WrongPassword()
        {
            var oldEncodedPasswordAnswer = "abcd354b23b5==";
            var newPassword = "12sdgjkl325";

            var user = new PfMembershipUser
            {
                Password = oldEncodedPasswordAnswer
            };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword("passowrd", user)).Return(false);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangePasswordQuestionAndAnswer(
                "12345", "passowrd", "What's my favorite color", newPassword, false);

            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.WrongPasswordSupplied));
            Assert.That(user.Password, Is.EqualTo(oldEncodedPasswordAnswer));
        }

        [Test]
        public void ChangePasswordQuestionAndAnswer_Fails_If_User_Not_Active()
        {
            var newPassword = "12sdgjkl325";

            var user = MockRepository.GenerateMock<PfMembershipUser>();
            user.Expect(x => x.ActiveUser).Return(false);

            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            var result = service.ChangePasswordQuestionAndAnswer(
                "12345", "passowrd", "What's my favorite color", newPassword, false);

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.InactiveUser));
            Assert.That(user.Password, Is.Null);
        }

        [Test]
        public void ChangePasswordQuestionAndAnswer_HappyPath()
        {
            var oldEncodedAnswer = "abcd354b23b5==";
            var newEncodedAnswer = "efd999923b2==";
            var newAnswer = "12sdgjkl325";

            var user = new PfMembershipUser
            {
                PasswordAnswer = oldEncodedAnswer
            };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword("passowrd", user)).Return(true);
            passwordService.Expect(x => x.EncodeSecureInformation(newAnswer)).Return(newEncodedAnswer);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangePasswordQuestionAndAnswer(
                "12345", "passowrd", "What's my favorite color", newAnswer, false);

            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.Success));
            Assert.That(user.PasswordAnswer, Is.EqualTo(newEncodedAnswer));
        }

        [Test]
        public void ChangePasswordQuestionAndAnswer_HappyPath_Admin_Override_Ignores_BadPassword_And_InactiveUser()
        {
            var oldEncodedAnswer = "abcd354b23b5==";
            var newEncodedAnswer = "efd999923b2==";
            var newAnswer = "12sdgjkl325";

            var user = new PfMembershipUser
            {
                PasswordAnswer = oldEncodedAnswer,
                IsApproved = false,
            };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.EncodeSecureInformation(newAnswer)).Return(newEncodedAnswer);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangePasswordQuestionAndAnswer(
                "12345", "passowrd", "What's my favorite color", newAnswer, true);

            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.Success));
            Assert.That(user.PasswordAnswer, Is.EqualTo(newEncodedAnswer));
        }


        [Test]
        public void ChangeEmailAddress_Fails_If_WrongPasswordGiven()
        {
            var oldPassword = "oldPassword123";
            var user = new PfMembershipUser
            {
                IsApproved = true,
                IsLockedOut = false,
            };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword(oldPassword, user)).Return(false);
            
            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangeEmailAddress("12345", oldPassword, "jones@jones.com", false);

            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.WrongPasswordSupplied));
        }

        [Test]
        public void ChangeEmailAddress_Fails_For_InactiveUser()
        {
            var oldPassword = "oldPassword123";
            var user = MockRepository.GenerateMock<PfMembershipUser>();
            user.Expect(x => x.ActiveUser).Return(false);

            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            var result = service.ChangeEmailAddress("12345", oldPassword, "jones@jones.com", false);

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.InactiveUser));
        }

        [Test]
        public void ChangeEmailAddress_Fails_For_Another_User_Having_Same_New_EmailAddress()
        {
            var oldPassword = "oldPassword123";
            var newEmailAddress = "jon@jones.com";
            var user = new PfMembershipUser
            {
                IsApproved = true,
                IsLockedOut = false,
            };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword(oldPassword, user)).Return(true);

            repository
                .Expect(x => x.GetUserByEmail(newEmailAddress))
                .Return(new PfMembershipUser());

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangeEmailAddress("12345", oldPassword, newEmailAddress, false);

            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.EmailAddressAlreadyTaken));
        }

        [Test]
        public void ChangeEmailAddress_Ignores_Password_With_AdminOverride_And_Succeeds()
        {
            var oldPassword = "oldPassword123";
            var newEmailAddress = "jon@jones.com";
            var user = new PfMembershipUser
            {
                IsApproved = true,
                IsLockedOut = false,
            };

            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);
            repository
                .Expect(x => x.GetUserByEmail(newEmailAddress))
                .Return(null);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            var result = service.ChangeEmailAddress("12345", oldPassword, newEmailAddress, true);

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.Success));
        }

        [Test]
        public void ChangeEmailAddress_Succeeds()
        {
            var oldPassword = "oldPassword123";
            var newEmailAddress = "jon@jones.com";
            var user = new PfMembershipUser
            {
                IsApproved = true,
                IsLockedOut = false,
            };
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            var passwordService = MockRepository.GenerateMock<IPfPasswordService>();
            passwordService.Expect(x => x.CheckPassword(oldPassword, user)).Return(true);

            repository
                .Expect(x => x.GetUserByEmail(newEmailAddress))
                .Return(null);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, passwordService, null);
            var result = service.ChangeEmailAddress("12345", oldPassword, newEmailAddress, false);

            // Assert
            repository.VerifyAllExpectations();
            passwordService.VerifyAllExpectations();
            Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.Success));
        }


        [Test]
        public void Set_UserApproval_Invokes_WritableRepository_And_Changes_IsApproved()
        {
            var user = new PfMembershipUser
            {
                IsApproved = false,
                IsLockedOut = false,
            };
            var expectedIsApproved = true;
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            service.SetUserApproval("12345", expectedIsApproved);

            // Assert
            repository.VerifyAllExpectations();
            Assert.That(user.IsApproved, Is.EqualTo(expectedIsApproved));
        }

        [Test]
        public void TouchUser_Updates_LastActivityDate()
        {
            var user = new PfMembershipUser
            {
                IsApproved = false,
                IsLockedOut = false,
                LastActivityDate = new DateTime(2009, 1, 1),
            };

            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.GetUserByUserName("12345"))
                .Return(user);

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            service.Touch("12345");
            
            // Assert
            repository.VerifyAllExpectations();
            Assert.That(user.LastActivityDate, Is.GreaterThanOrEqualTo(DateTime.Now.AddMinutes(-2)));
        }

        [Test]
        public void DeleteUser_Invokes_Writable_Repository()
        {
            var repository = MockRepository.GenerateMock<IMembershipWritableRepository>();
            repository
                .Expect(x => x.DeleteUser("12345", true));

            // Act
            IPfMembershipService service = new PfMembershipService(null, repository, null, null);
            service.DeleteUser("12345");

            // Assert
            repository.VerifyAllExpectations();
        }

    }
}
