using System;
using System.Collections.Generic;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;


namespace Pleiades.Web.Security.Concrete
{
    /// <summary>
    /// Membership functions re-coded to be test-friendly and DI-friendly
    /// </summary>
    public class PfMembershipService : IPfMembershipService
    {
        public IMembershipRepository Repository;
        public IPfPasswordService PasswordServices;
        public IPfMembershipSettings Settings;

        const string ApplicationName = "/";

        public PfMembershipService(IMembershipRepository repository, IPfPasswordService passwordServices, IPfMembershipSettings settings)
        {
            this.Repository = repository;
            this.PasswordServices = passwordServices;
            this.Settings = settings;
        }

        public PfMembershipUser CreateUser(CreateNewMembershipUserRequest request, out PfMembershipCreateStatus createStatus)
        {
            var validPassword = PasswordServices.IsValidPassword(request.Password);
            if (!validPassword)
            {
                createStatus = PfMembershipCreateStatus.InvalidPassword;
                return null;
            }

            var userByEmail = Repository.GetUserByEmail(request.Email);
            if (userByEmail != null)
            {
                createStatus = PfMembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var userByUserName = Repository.GetUserByUserName(request.UserName);
            if (userByUserName != null)
            {
                createStatus = PfMembershipCreateStatus.DuplicateUserName;
                return null;
            }

            var createDate = DateTime.Now;
            var user = new PfMembershipUser 
            {
                ProviderUserKey = Guid.Empty,
                UserName = request.UserName,
                ApplicationName = ApplicationName,
                Email = request.Email,
                Password = this.PasswordServices.EncodePassword(request.Password),
                PasswordQuestion = request.PasswordQuestion,
                PasswordAnswer = request.PasswordAnswer,
                IsApproved = request.IsApproved,
                LastActivityDate = createDate,
                LastLoginDate = createDate,
                LastPasswordChangedDate = createDate,
                CreationDate = createDate,
                IsOnline = false,
                IsLockedOut = false,
                LastLockedOutDate = createDate,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAttemptWindowStart = createDate,
                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = createDate,
                LastModified = createDate,
            };

            Repository.Insert(user);
            createStatus = PfMembershipCreateStatus.Success;
            return user;
        }

        public string GenerateUniqueUserName(int maxAttempts)
        {
            var counter = 1;
            while (counter++ < maxAttempts)
            {
                var username = "";
                var random = new Random();
                username = random.Next(9999999).ToString("D7");
                    
                if (Repository.GetUserByUserName(username) == null)
                {
                    return username;
                }
            }

            throw new Exception("Unable to generate new user name after " + maxAttempts + " tries");
        }

        public PfMembershipUser ValidateUserByEmailAddr(string emailAddress, string password)
        {
            var user = this.Repository.GetUserByEmail(emailAddress);
            if (user == null)
            {
                return null;
            }

            if (!user.IsApproved || user.IsLockedOut)
            {
                return null;
            }

            if (this.PasswordServices.CheckSecureInformation(password, user.Password))
            {
                user.LastLoginDate = DateTime.Now;
                return user;
            }
            else
            {
                this.PasswordServices.UpdateFailedPasswordAttempts(user);
                return null;
            }
        }

        public PfMembershipUser GetUserByEmail(string emailAddress)
        {
            return this.Repository.GetUserByEmail(emailAddress);
        }

        public PfMembershipUser GetUserByUserName(string username)
        {
            return this.Repository.GetUserByUserName(username);
        }

        public IEnumerable<PfMembershipUser> GetAllUsers()
        {
            return this.Repository.GetAllUsers();
        }

        public int GetNumberOfUsersOnline()
        {
            return this.Repository.GetNumberOfUsersOnline(Settings.UserIsOnlineTimeWindow);
        }

        public void UnlockUser(string username)
        {
            var user = this.Repository.GetUserByUserName(username);
            user.IsLockedOut = false;
        }

        public string PasswordQuestion(string username)
        {
            var user = this.Repository.GetUserByUserName(username);
            return user.PasswordQuestion;
        }

        public string ResetPassword(string username, string answer, bool adminOverride, out PfPasswordChangeStatus status)
        {
            var user = this.Repository.GetUserByUserName(username);
            if (user == null)
            {
                status = PfPasswordChangeStatus.UserDoesNotExist;
                return null;
            }
            if (!user.IsApproved)
            {
                status = PfPasswordChangeStatus.UserIsNotApproved;
                return null;
            }
            if (user.IsLockedOut)
            {
                status = PfPasswordChangeStatus.UserIsLockedOut;
                return null;
            }

            if (!adminOverride)
            {
                if (!Settings.EnablePasswordReset)
                {
                    status = PfPasswordChangeStatus.PasswordResetIsNotEnabled;
                    return null;
                }
                if (Settings.RequiresQuestionAndAnswer)
                {
                    if (answer == null)
                    {
                        PasswordServices.UpdateFailedQuestionAndAnswerAttempts(user);
                        status = PfPasswordChangeStatus.AnswerRequiredForPasswordReset;
                        return null;
                    }
                    if (!PasswordServices.CheckSecureInformation(answer, user.PasswordAnswer))
                    {
                        PasswordServices.UpdateFailedQuestionAndAnswerAttempts(user);
                        status = PfPasswordChangeStatus.WrongAnswerSuppliedForSecurityQuestion;
                        return null;
                    }
                }
            }

            string newPassword = PasswordServices.GeneratePassword();
            user.Password = PasswordServices.EncodePassword(newPassword);
            user.LastPasswordChangedDate = DateTime.Now;
            status = PfPasswordChangeStatus.Success;
            return newPassword;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword, bool adminOverride, out PfPasswordChangeStatus status)
        {
            throw new NotImplementedException();   
        }

        // Totally different!
        public bool ChangePasswordQuestionAndAnswer(string username, string password, string question, string answer, out string message)
        {
            var user = this.Repository.GetUserByUserName(username);
            throw new NotImplementedException();   
        }
        
        [Obsolete]
        public bool ChangeEmailAddress(string username, string emailAddress, out string message)
        {
            var user = this.Repository.GetUserByUserName(username);
            var userCollision = this.Repository.GetUserByEmail(emailAddress);
            if (userCollision != null)
            {
                message = "User with Email Address: " + emailAddress + " exists already";
            }
            user.Email = emailAddress;
            throw new NotImplementedException();
        }

        public bool ChangeEmailAddress(string userName, string password, string emailAddress, out string message)
        {
            throw new NotImplementedException();
        }

        public void SetUserApproval(string username, bool approved)
        {
            var user = this.Repository.GetUserByUserName(username);
            user.IsApproved = approved;
        }

        public void Touch(string username)
        {
            var user = this.Repository.GetUserByUserName(username);
            user.LastActivityDate = DateTime.Now;
        }

        // Soft Delete, mayhaps?
        public void DeleteUser(string username)
        {
            this.Repository.DeleteUser(username, true);
        }

    }
}
