using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IPfPasswordService
    {
        bool IsValidPassword(string password);
        string EncodePassword(string password);
        bool CheckSecureInformation(string submittedUnencodedPassword, string encodedPassword);
        string GeneratePassword();
        void UpdateFailedPasswordAttempts(PfMembershipUser user);
        void UpdateFailedQuestionAndAnswerAttempts(PfMembershipUser user);
    }
}
