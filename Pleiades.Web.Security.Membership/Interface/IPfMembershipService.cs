using System.Collections.Generic;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Contains all Membership Provider-related operations for managing Membership User objects
    /// </summary>
    public interface IPfMembershipService
    {
        PfMembershipUser CreateUser(CreateNewMembershipUserRequest request, out PfMembershipCreateStatus outCreateStatus);
        string GenerateUniqueUserName(int maxAttempts);

        PfMembershipUser ValidateUserByEmailAddr(string emailaddr, string password);
        PfMembershipUser GetUserByEmail(string emailAddress);
        PfMembershipUser GetUserByUserName(string userName);
        IEnumerable<PfMembershipUser> GetAllUsers();        
        int GetNumberOfUsersOnline();

        void UnlockUser(string userName);
        string PasswordQuestion(string userName);

        string ResetPassword(string userName, string answer, bool adminOverride, out PfPasswordChangeStatus status);
        bool ChangePassword(string userName, string oldPassword, string newPassword, bool adminOverride, out PfPasswordChangeStatus status);

        bool ChangePasswordQuestionAndAnswer(string userName, string password, string question, string answer, out string message);
        bool ChangeEmailAddress(string userName, string password, string emailAddress, out string message);

        void SetUserApproval(string userName, bool approved);
        void Touch(string userName);
        void DeleteUser(string userName);
    }
}
