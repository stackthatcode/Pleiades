using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IPfPasswordService
    {
        bool IsValidPassword(string password);
        string EncodeSecureInformation(string password);
        bool CheckPassword(string unencodedPassword, PfMembershipUser user);
        bool CheckPasswordAnswer(string unencodedAnswer, PfMembershipUser user);
        string GeneratePassword();
        void UpdateFailedPasswordAttempts(PfMembershipUser user);
        void UpdateFailedQuestionAndAnswerAttempts(PfMembershipUser user);
    }
}
