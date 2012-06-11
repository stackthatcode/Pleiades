using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Web.Security.Utility;

namespace Pleiades.Framework.Web.Security.Concrete
{
    /// <summary>
    /// 
    /// </summary>
    public class MembershipAdapter : IMembershipAdapter
    {
        // TODO: eliminate this...?
        public IDomainUserService DomainUserService { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public MembershipAdapter()
        {
            DomainUserService = new DomainUserService();
        }

        /// <summary>
        /// Validate User Credentials
        /// </summary>
        /// <returns>A non-null Domain User if Validation succeeds</returns>
        public Model.DomainUser ValidateUserByEmailAddr(string emailaddr, string password)
        {
            var user = DomainUserService.RetrieveUserByEmail(emailaddr);
            if (user == null)
            {
                return null;
            }

            if (user.AccountStatus == Model.AccountStatus.Disabled)
            {
                return null;
            }

            var membershipValidated = Membership.ValidateUser(user.MembershipUser.UserName, password);
            if (membershipValidated == false)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Get the current number of User Active online - "touched" in the last configurable period
        /// </summary>
        public int GetNumberOfUsersOnline()
        {
            return Membership.GetNumberOfUsersOnline();
        }

        /// <summary>
        /// Unlock a User Account that's failed Validation too many times
        /// </summary>
        public void UnlockUser(string userName)
        {
            var membershipUser = Membership.GetUser(userName);
            membershipUser.UnlockUser();
            this.DomainUserService.UpdateLastModified(user);
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        public string ResetPassword(Model.DomainUser user)
        {
            var membershipUser = Membership.GetUser(user.MembershipUser.UserName);
            var resetPassword = membershipUser.ResetPassword();
            DomainUserService.UpdateLastModified(user);
            return resetPassword;
        }

        /// <summary>
        /// Reset Password with Answer for Password Question
        /// </summary>
        public string ResetPasswordWithAnswer(Model.DomainUser user, string answer)
        {
            var membershipUser = Membership.GetUser(user.MembershipUser.UserName);
            var resetPassword = membershipUser.ResetPassword(answer);
            DomainUserService.UpdateLastModified(user);
            return resetPassword;
        }

        /// <summary>
        /// Retreive Password Question
        /// </summary>
        public string PasswordQuestion(Model.DomainUser user)
        {
            var membershipUser = Membership.GetUser(user.MembershipUser.UserName);
            return membershipUser.PasswordQuestion;
        }

        /// <summary>
        /// Change Domain User's Password
        /// </summary>
        public void ChangePassword(Model.DomainUser user, string oldPassword, string newPassword)
        {
            var membershipUser = Membership.GetUser(user.MembershipUser.UserName);
            if (!membershipUser.ChangePassword(oldPassword, newPassword))
            {
                throw new Exception("Internal error occurs while attempting to change password");
            }

            DomainUserService.UpdateLastModified(user);
        }

        /// <summary>
        /// Change Domain User's Password with Question and Answer
        /// </summary>
        public void ChangePasswordQuestionAndAnswer(Model.DomainUser user, string password, string question, string answer)
        {
            var membershipUser = Membership.GetUser(user.MembershipUser.UserName);
            if (!membershipUser.ChangePasswordQuestionAndAnswer(password, question, answer))
            {
                throw new Exception("Internal error occurs while attempting to change password");
            }

            DomainUserService.UpdateLastModified(user);
        }

        /// <summary>
        /// Change Domain User's Email Address
        /// </summary>
        public void ChangeEmailAddress(Model.DomainUser user, string emailAddress)
        {
            var userName = Membership.GetUserNameByEmail(emailAddress);
            if (userName == user.MembershipUser.UserName)
                return;
            if (userName != "")
            {
                throw new Exception("A User Account already has that Email Address"); 
            }

            var membershipUser = Membership.GetUser(user.MembershipUser.UserName);
            membershipUser.Email = emailAddress;
            Membership.UpdateUser(membershipUser);

            DomainUserService.UpdateLastModified(user);
        }

        /// <summary>
        /// Change the User's Membership Approval
        /// </summary>
        public void SetUserApproval(Model.DomainUser user, bool approved)
        {
            var membershipUser = Membership.GetUser(user.MembershipUser.UserName);
            membershipUser.IsApproved = approved;
            Membership.UpdateUser(membershipUser);

            DomainUserService.UpdateLastModified(user);
        }

        /// <summary>
        /// Signal User Activity has occured
        /// </summary>
        /// <param name="user"></param>
        public void Touch(Model.DomainUser user)
        {
            var membershipUser = Membership.GetUser(user.MembershipUser.UserName);
        }
          
    }
}
