using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using SecurityMembershipUser = System.Web.Security.MembershipUser;
using Pleiades.Data;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Providers;

namespace Pleiades.Web.Security.Concrete
{
    /// <summary>
    /// Wrapper around common Membership functions - enables testability of Membership-related functions
    /// </summary>
    public class MembershipService : IMembershipService
    {
        IMembershipProviderRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public MembershipService(IMembershipProviderRepository repository, IUnitOfWork unitOfWork)
        {
            this.Repository = repository;
            this.UnitOfWork = unitOfWork;
        }

        public Model.MembershipUser CreateUser(
                CreateNewMembershipUserRequest request, out PleiadesMembershipCreateStatus outCreateStatus)
        {
            MembershipCreateStatus createStatus;
            var generatedUserName = GenerateUserName();
            Membership.CreateUser(
                generatedUserName, request.Password, request.Email, request.PasswordQuestion, request.PasswordAnswer, 
                request.IsApproved, out createStatus);
            this.UnitOfWork.SaveChanges();   // I HATE THIS - THE SERVICES ARE MANAGING THE UNIT OF WORK TO WHICH THEY BELONG!!!

            outCreateStatus = (PleiadesMembershipCreateStatus)createStatus;
            return Repository.GetUser(generatedUserName);
        }

        /// <summary>
        /// Creates a random 7-digit number for User Name (since we authenticate by email addy)
        /// </summary>
        public string GenerateUserName()
        {
            var username = "";
            var counter = 0;
            while (counter++ < 100)
            {
                var random = new Random();
                username = random.Next(9999999).ToString("D7");

                if (Membership.FindUsersByName(username).Count == 0)
                {
                    return username;
                }
            }

            throw new Exception("Unable to generate new user name after 100 tries");
        }

        /// <summary>
        /// Returns a non-null Membership User if Validation succeeds
        /// </summary>
        public Model.MembershipUser ValidateUserByEmailAddr(string emailAddress, string password)
        {
            var user = GetSingleUserByEmail(emailAddress);           
            if (user == null)
            {
                return null;
            }

            var membershipValidated = Membership.ValidateUser(user.UserName, password);
            if (membershipValidated == false)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Returns user if they exist in Membership.  Throws an exception if there's more than one match.
        /// </summary>
        public Model.MembershipUser GetSingleUserByEmail(string emailAddress)
        {
            var users =  Membership.FindUsersByEmail(emailAddress);
            if (users.Count == 0)
            {
                return null;
            }
            if (users.Count > 1)
            {
                throw new Exception("Multiple users were found with email address: " + emailAddress);
            }

            var enumerator = users.GetEnumerator();
            enumerator.MoveNext();
            var user = (SecurityMembershipUser)enumerator.Current;
            return user.ToModelMembershipUser();
        }

        /// <summary>
        /// Retrieves username by email address.  Returns null if User does not exist in Membership
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public string GetUserNameByEmail(string emailAddress)
        {
            var user = GetSingleUserByEmail(emailAddress);
            if (user == null)
            {
                return null;
            }
            return user.UserName;
        }

        // TODO: wire in the Repository directly - eliminate the legacy
        public List<Model.MembershipUser> GetAllUsers()
        {            
            int totalRecords;
            var result = Membership.GetAllUsers(1, System.Int32.MaxValue, out totalRecords);
            var output = new List<Model.MembershipUser>();
            foreach (var user in result)
            {
                var modelUser = ((SecurityMembershipUser)user).ToModelMembershipUser();
                output.Add(modelUser);
            }
            return output;
        }

        // TODO: wire in the Repository directly - eliminate the legacy
        public Model.MembershipUser GetUserByUserName(string username)
        {
            var users = Membership.FindUsersByName(username);
            if (users.Count == 0)
            {
                return null;
            }
            if (users.Count > 1)
            {
                throw new Exception("Multiple users were found with username: " + username);
            }

            var user = (SecurityMembershipUser)users.GetEnumerator().Current;
            return user.ToModelMembershipUser();
        }

        /// <summary>
        /// Get the current number of User Active online - "touched" in the last configurable period
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public int GetNumberOfUsersOnline()
        {
            return Membership.GetNumberOfUsersOnline();
        }

        /// <summary>
        /// Unlock a User Account that's failed Validation too many times
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public void UnlockUser(string username)
        {
            var membershipUser = Membership.GetUser(username);
            membershipUser.UnlockUser();
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public string ResetPassword(string username)
        {
            var membershipUser = Membership.GetUser(username);
            var resetPassword = membershipUser.ResetPassword();     // How did I know to did this...???
            return resetPassword;
        }

        /// <summary>
        /// Reset Password with Answer for Password Question
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public string ResetPasswordWithAnswer(string username, string answer)
        {
            var membershipUser = Membership.GetUser(username);
            var resetPassword = membershipUser.ResetPassword(answer);
            return resetPassword;
        }

        /// <summary>
        /// Retreive Password Question
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public string PasswordQuestion(string username)
        {
            var membershipUser = Membership.GetUser(username);
            return membershipUser.PasswordQuestion;
        }

        // TODO: wire in the Repository directly - eliminate the legacy
        public void DeleteUser(string username)
        {
            Membership.DeleteUser(username);
        }

        /// <summary>
        /// Change Membership User's Password
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            var membershipUser = Membership.GetUser(username);
            if (!membershipUser.ChangePassword(oldPassword, newPassword))
            {
                throw new Exception("Internal error occurs while attempting to change password");
            }
        }

        /// <summary>
        /// Change Membership User's Password with Question and Answer
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public void ChangePasswordQuestionAndAnswer(string username, string password, string question, string answer)
        {
            var membershipUser = Membership.GetUser(username);
            if (!membershipUser.ChangePasswordQuestionAndAnswer(password, question, answer))
            {
                throw new Exception("Internal error occurs while attempting to change password");
            }
        }

        /// <summary>
        /// Change Membership User's Email Address
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public void ChangeEmailAddress(string username, string emailAddress)
        {
            var membershipUser = Membership.GetUser(username);
            membershipUser.Email = emailAddress;
            Membership.UpdateUser(membershipUser);
        }

        /// <summary>
        /// Change the User's Membership Approval
        /// </summary>
        // TODO: wire in the Repository directly - eliminate the legacy
        public void SetUserApproval(string username, bool approved)
        {
            var membershipUser = Membership.GetUser(username);
            membershipUser.IsApproved = approved;
            Membership.UpdateUser(membershipUser);
        }

        /// <summary>
        /// Signal User Activity has occured
        /// </summary>
        /// <param name="user"></param>
        // TODO: wire in the Repository directly - eliminate the legacy
        public void Touch(string username)
        {
            var membershipUser = Membership.GetUser(username);
        }          
    }
}
