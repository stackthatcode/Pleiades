namespace Pleiades.Framework.MembershipProvider.Model
{
    public enum PleiadesMembershipCreateStatus
    {
        Success = 0,
        InvalidUserName = 1,
        InvalidPassword = 2,
        InvalidQuestion = 3,
        InvalidAnswer = 4,
        InvalidEmail = 5,
        DuplicateUserName = 6,
        DuplicateEmail = 7,
        UserRejected = 8,
        InvalidProviderUserKey = 9,
        DuplicateProviderUserKey = 10,
        ProviderError = 11,
    }
}
