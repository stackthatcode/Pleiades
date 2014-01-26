using System;

namespace Commerce.Application.Security
{
    public interface IPasswordResetLinkRepository
    {
        void Add(PasswordResetLink passwordResetLink);
        PasswordResetLink Retrieve(Guid guid);
    }
}
