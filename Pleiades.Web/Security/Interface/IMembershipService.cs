using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contains all Membership Provider-related operations for managing Domain Users
    /// </summary>
    public interface IMembershipService
    {
        DomainUser ValidateUserByEmailAddr(string emailaddr, string password);
        void Touch(Model.DomainUser user);
        int GetNumberOfUsersOnline();
        void UnlockUser(DomainUser user);
        string ResetPassword(DomainUser user);
        string ResetPasswordWithAnswer(DomainUser user, string answer);
        string PasswordQuestion(DomainUser user);
        void ChangePassword(DomainUser user, string oldPassword, string newPassword);
        void ChangePasswordQuestionAndAnswer(DomainUser user, string password, string question, string answer);
        void ChangeEmailAddress(DomainUser user, string emailAddress);
        void SetUserApproval(DomainUser user, bool approved);
    }
}