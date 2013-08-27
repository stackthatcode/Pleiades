using System.Collections.Generic;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contains all Membership Provider-related operations for managing Membership User objects
    /// </summary>
    public interface IPfMembershipService
    {
        PfMembershipUser CreateUser(PfCreateNewMembershipUserRequest request, out PfMembershipCreateStatus outCreateStatus);
        string GenerateUniqueUserName(int maxAttempts);

        PfMembershipUser ValidateUserByEmailAddr(string emailaddr, string password);
        PfMembershipUser GetUserByEmail(string emailAddress);
        PfMembershipUser GetUserByUserName(string userName);
        string PasswordQuestion(string userName);
        IEnumerable<PfMembershipUser> GetAllUsers();        
        int GetNumberOfUsersOnline();

        string ResetPassword(string userName, string answer, bool adminOverride, out PfCredentialsChangeStatus status);
        PfCredentialsChangeStatus ChangePassword(string userName, string oldPassword, string newPassword, bool adminOverride);
        PfCredentialsChangeStatus ChangePasswordQuestionAndAnswer(
            string userName, string password, string question, string answer, bool adminOverride);
        PfCredentialsChangeStatus ChangeEmailAddress(string userName, string password, string emailAddress, bool adminOverride);

        void UnlockUser(string userName);
        void SetUserApproval(string userName, bool approved);
        void Touch(string userName);
        void DeleteUser(string userName);
    }
}
