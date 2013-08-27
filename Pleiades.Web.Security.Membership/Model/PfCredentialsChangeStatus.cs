namespace Pleiades.Web.Security.Model
{
    public enum PfCredentialsChangeStatus
    {
        Success = 0,
        PasswordResetIsNotEnabled = 1,
        AnswerRequiredForPasswordReset = 2,
        WrongAnswerSupplied = 3,
        WrongPasswordSupplied = 4,
        EmailAddressAlreadyTaken = 5,
        InactiveUser = 6,
    }
}
