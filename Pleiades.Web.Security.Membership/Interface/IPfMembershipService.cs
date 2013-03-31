﻿using System;
using System.Collections.Generic;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contains all Membership Provider-related operations for managing Membership User objects -- root Entity Aggregates
    /// </summary>
    public interface IPfMembershipService
    {
        PfMembershipUser CreateUser(CreateNewMembershipUserRequest request, out PleiadesMembershipCreateStatus outCreateStatus);
        string GenerateUserName();
        PfMembershipUser ValidateUserByEmailAddr(string emailaddr, string password);
        PfMembershipUser GetSingleUserByEmail(string emailAddress);
        PfMembershipUser GetUserByUserName(string userName);
        List<PfMembershipUser> GetAllUsers();
        
        string GetUserNameByEmail(string emailAddress);
        int GetNumberOfUsersOnline();

        void UnlockUser(string userName);
        string ResetPassword(string userName);
        string ResetPasswordWithAnswer(string userName, string answer);
        string PasswordQuestion(string userName);

        void DeleteUser(string userName);

        void ChangePassword(string userName, string oldPassword, string newPassword);
        void ChangePasswordQuestionAndAnswer(string userName, string password, string question, string answer);
        void ChangeEmailAddress(string userName, string emailAddress);
        void SetUserApproval(string userName, bool approved);
        void Touch(string userName);
    }
}