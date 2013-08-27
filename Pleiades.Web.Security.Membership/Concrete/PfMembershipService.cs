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
        public IMembershipReadOnlyRepository ReadOnlyRepository;
        public IMembershipWritableRepository WritableRepository;
        public IPfPasswordService PasswordServices;
        public IPfMembershipSettings Settings;

        const string ApplicationName = "/";

        public PfMembershipService(
                IMembershipReadOnlyRepository readOnlyRepository, IMembershipWritableRepository writableRepository, 
                IPfPasswordService passwordServices, IPfMembershipSettings settings)
        {
            this.ReadOnlyRepository = readOnlyRepository;
            this.WritableRepository = writableRepository;
            this.PasswordServices = passwordServices;
            this.Settings = settings;
        }

        public PfMembershipUser CreateUser(PfCreateNewMembershipUserRequest request, out PfMembershipCreateStatus createStatus)
        {
            var validPassword = PasswordServices.IsValidPassword(request.Password);
            if (!validPassword)
            {
                createStatus = PfMembershipCreateStatus.InvalidPassword;
                return null;
            }

            var userByEmail = ReadOnlyRepository.GetUserByEmail(request.Email);
            if (userByEmail != null)
            {
                createStatus = PfMembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var userByUserName = ReadOnlyRepository.GetUserByUserName(request.UserName);
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
                Password = this.PasswordServices.EncodeSecureInformation(request.Password),
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

            WritableRepository.AddUser(user);
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
                    
                if (ReadOnlyRepository.GetUserByUserName(username) == null)
                {
                    return username;
                }
            }

            throw new Exception("Unable to generate new user name after " + maxAttempts + " tries");
        }

        public PfMembershipUser ValidateUserByEmailAddr(string emailAddress, string password)
        {
            var user = this.ReadOnlyRepository.GetUserByEmail(emailAddress);
            if (user == null)
            {
                return null;
            }

            if (!user.IsApproved || user.IsLockedOut)
            {
                return null;
            }

            if (this.PasswordServices.CheckPassword(password, user))
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
            return this.ReadOnlyRepository.GetUserByEmail(emailAddress);
        }

        public PfMembershipUser GetUserByUserName(string username)
        {
            return this.ReadOnlyRepository.GetUserByUserName(username);
        }

        public IEnumerable<PfMembershipUser> GetAllUsers()
        {
            return this.ReadOnlyRepository.GetAllUsers();
        }

        public int GetNumberOfUsersOnline()
        {
            return this.ReadOnlyRepository.GetNumberOfUsersOnline(Settings.UserIsOnlineTimeWindow);
        }

        public string PasswordQuestion(string username)
        {
            var user = this.ReadOnlyRepository.GetUserByUserName(username);
            return user.PasswordQuestion;
        }

        public void UnlockUser(string username)
        {
            var user = this.WritableRepository.GetUserByUserName(username);
            user.IsLockedOut = false;
        }

        public string ResetPassword(string username, string answer, bool adminOverride, out PfCredentialsChangeStatus status)
        {
            var user = this.WritableRepository.GetUserByUserName(username);
            if (!adminOverride)
            {
                if (!user.ActiveUser)
                {
                    status = PfCredentialsChangeStatus.InactiveUser;
                    return null;
                }
                if (!Settings.EnablePasswordReset)
                {
                    status = PfCredentialsChangeStatus.PasswordResetIsNotEnabled;
                    return null;
                }
                if (Settings.RequiresQuestionAndAnswer)
                {
                    if (answer == null)
                    {
                        PasswordServices.UpdateFailedQuestionAndAnswerAttempts(user);
                        status = PfCredentialsChangeStatus.AnswerRequiredForPasswordReset;
                        return null;
                    }
                    if (!PasswordServices.CheckPasswordAnswer(answer, user))
                    {
                        PasswordServices.UpdateFailedQuestionAndAnswerAttempts(user);
                        status = PfCredentialsChangeStatus.WrongAnswerSupplied;
                        return null;
                    }
                }
            }

            string newPassword = PasswordServices.GeneratePassword();
            user.Password = PasswordServices.EncodeSecureInformation(newPassword);
            user.LastPasswordChangedDate = DateTime.Now;
            user.LastModified = DateTime.Now;
            status = PfCredentialsChangeStatus.Success;
            return newPassword;
        }

        public PfCredentialsChangeStatus ChangePassword(string username, string oldPassword, string newPassword, bool adminOverride)
        {
            var user = this.WritableRepository.GetUserByUserName(username);
            if (!adminOverride)
            {
                if (!user.ActiveUser)
                {
                    return PfCredentialsChangeStatus.InactiveUser;
                }
                if (!PasswordServices.CheckPassword(oldPassword, user))
                {
                    PasswordServices.UpdateFailedQuestionAndAnswerAttempts(user);
                    return PfCredentialsChangeStatus.WrongPasswordSupplied;
                }
            }

            user.Password = PasswordServices.EncodeSecureInformation(newPassword);
            user.LastPasswordChangedDate = DateTime.Now;
            user.LastModified = DateTime.Now;
            return PfCredentialsChangeStatus.Success;
        }

        public PfCredentialsChangeStatus ChangePasswordQuestionAndAnswer(
                string userName, string password, string question, string answer, bool adminOverride)
        {
            var user = this.WritableRepository.GetUserByUserName(userName);            
            if (!adminOverride)
            {
                if (!user.ActiveUser)
                {
                    return PfCredentialsChangeStatus.InactiveUser; ;
                }
                if (!PasswordServices.CheckPassword(password, user))
                {
                    PasswordServices.UpdateFailedQuestionAndAnswerAttempts(user);
                    return PfCredentialsChangeStatus.WrongPasswordSupplied;
                }
            }

            user.PasswordAnswer = PasswordServices.EncodeSecureInformation(answer);
            user.LastModified = DateTime.Now;
            return PfCredentialsChangeStatus.Success;
        }

        public PfCredentialsChangeStatus ChangeEmailAddress(string userName, string password, string emailAddress, bool adminOverride)
        {
            var user = this.WritableRepository.GetUserByUserName(userName);
            var userCollision = this.WritableRepository.GetUserByEmail(emailAddress);
            if (!adminOverride)
            {
                if (!user.ActiveUser)
                {
                    return PfCredentialsChangeStatus.InactiveUser;
                }
                if (!this.PasswordServices.CheckPassword(password, user))
                {
                    return PfCredentialsChangeStatus.WrongPasswordSupplied;
                }
            }
            if (userCollision != null)
            {
                return PfCredentialsChangeStatus.EmailAddressAlreadyTaken;
            }
            user.Email = emailAddress;            
            return PfCredentialsChangeStatus.Success;
        }

        public void SetUserApproval(string username, bool approved)
        {
            var user = this.WritableRepository.GetUserByUserName(username);
            user.IsApproved = approved;
        }

        public void Touch(string username)
        {
            var user = this.WritableRepository.GetUserByUserName(username);
            user.LastActivityDate = DateTime.Now;
        }

        // Soft Delete, mayhaps?
        public void DeleteUser(string username)
        {
            this.WritableRepository.DeleteUser(username, true);
        }
    }
}
