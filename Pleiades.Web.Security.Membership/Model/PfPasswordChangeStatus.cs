namespace Pleiades.Web.Security.Model
{
    public enum PfPasswordChangeStatus
    {
        Success = 0,
        UserDoesNotExist = 1,
        UserIsNotApproved = 2,
        UserIsLockedOut = 3,
        PasswordResetIsNotEnabled = 4,
        AnswerRequiredForPasswordReset = 5,
        WrongAnswerSuppliedForSecurityQuestion = 6,
    }
}
