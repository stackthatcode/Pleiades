using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
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
        public IPasswordServices PasswordServices;
        public IEncryptionService EncryptionService;

        const string ApplicationName = "/";

        public PfMembershipService(IMembershipRepository repository, IPasswordServices passwordServices, IEncryptionService encryptionService)
        {
            this.Repository = repository;
            this.PasswordServices = passwordServices;
            this.EncryptionService = encryptionService;
        }

        public PfMembershipUser CreateUser(CreateNewMembershipUserRequest request, out PfMembershipCreateStatus createStatus)
        {
            var validPassword = PasswordServices.IsValid(request.Password);
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

            if (CheckPassword(password, user))
            {
                if (user.IsApproved && !user.IsLockedOut)
                {
                    user.LastLoginDate = DateTime.Now;
                    return user;
                }
            }
            else
            {
                user.FailedPasswordAttemptCount++;
            }
            return null;
        }

        private bool CheckPassword(string password1, PfMembershipUser user)
        {
            return this.EncryptionService.MakeHMAC256Base64(password1) == user.Password;
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



        // Bring this in for the ValidateUser stuff

        // TODO: create PfMembershipConfiguration object

        // TODO: where do we store password attempt window...?


        //#region private methods
        ///// <summary>
        ///// A helper method that performs the checks and updates associated with password failure tracking.
        ///// </summary>
        //private void UpdateFailureCount(string username, string failureType)
        //{
        //    var repository = PfMembershipRepositoryBroker.Create();
        //    var user = repository.GetUser(username);

        //    if (user == null)
        //    {
        //        throw new ProviderException("Update failure count failed. No unique user found.");
        //    }

        //    var windowStart = new DateTime();
        //    int failureCount = 0;

        //    if (failureType == "password")
        //    {
        //        failureCount = Convert.ToInt32(user.FailedPasswordAttemptCount);
        //        windowStart = Convert.ToDateTime(user.FailedPasswordAttemptWindowStart);
        //    }

        //    if (failureType == "passwordAnswer")
        //    {
        //        failureCount = Convert.ToInt32(user.FailedPasswordAnswerAttemptCount);
        //        windowStart = Convert.ToDateTime(user.FailedPasswordAnswerAttemptWindowStart);
        //    }

        //    var windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

        //    if (failureCount == 0 || DateTime.Now > windowEnd)
        //    {
        //        // First password failure or outside of PasswordAttemptWindow. 
        //        // Start a new password failure count from 1 and a new window starting now.
        //        if (failureType == "password")
        //        {
        //            user.FailedPasswordAttemptCount = 1;
        //            user.FailedPasswordAttemptWindowStart = DateTime.Now;
        //        }
        //        if (failureType == "passwordAnswer")
        //        {
        //            user.FailedPasswordAnswerAttemptCount = 1;
        //            user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
        //        }
        //    }
        //    else
        //    {
        //        // FIXED: failure count
        //        failureCount++;
        //        if (failureCount >= MaxInvalidPasswordAttempts)
        //        {
        //            // Max password attempts have exceeded the failure threshold. Lock out the user.
        //            user.IsLockedOut = true;
        //            user.LastLockedOutDate = DateTime.Now;
        //            user.FailedPasswordAnswerAttemptCount = 5;
        //        }
        //        else
        //        {
        //            // Max password attempts have not exceeded the failure threshold. Update
        //            // the failure counts. Leave the window the same.
        //            if (failureType == "password")
        //            {
        //                user.FailedPasswordAttemptCount = failureCount;
        //            }
        //            if (failureType == "passwordAnswer")
        //            {
        //                user.FailedPasswordAnswerAttemptCount = failureCount;
        //            }
        //        }
        //    }
        //}



        public int GetNumberOfUsersOnline()
        {
            return Membership.GetNumberOfUsersOnline();
        }

        public void UnlockUser(string username)
        {
            var membershipUser = Membership.GetUser(username);
            membershipUser.UnlockUser();
        }

        public string ResetPassword(string username)
        {
            throw new NotImplementedException();
        }

        public string ResetPasswordWithAnswer(string username, string answer)
        {
            var membershipUser = Membership.GetUser(username);
            var resetPassword = membershipUser.ResetPassword(answer);
            return resetPassword;
        }

        public string PasswordQuestion(string username)
        {
            var user = this.Repository.GetUserByUserName(username);
            return user.PasswordQuestion;
        }

        // Soft Delete, mayhaps?
        public void DeleteUser(string username)
        {
            this.Repository.DeleteUser(username, true);
        }
        
        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void ChangePasswordQuestionAndAnswer(string username, string password, string question, string answer)
        {
            var user = this.Repository.GetUserByUserName(username);
            
            var membershipUser = Membership.GetUser(username);
            if (!membershipUser.ChangePasswordQuestionAndAnswer(password, question, answer))
            {
                throw new Exception("Internal error occurs while attempting to change password");
            }

            throw new NotImplementedException();
        }

        public void ChangeEmailAddress(string username, string emailAddress)
        {
            var user = this.Repository.GetUserByUserName(username);
            var userCollision = this.Repository.GetUserByEmail(emailAddress);

            if (userCollision != null)
            {
                throw new Exception("User with Email Address: " + emailAddress + " exists already");
            }

            user.Email = emailAddress;
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
    }
}
