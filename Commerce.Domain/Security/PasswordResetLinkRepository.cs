using System;
using System.Linq;
using Commerce.Application.Database;

namespace Commerce.Application.Security
{
    public class PasswordResetLinkRepository : IPasswordResetLinkRepository
    {
        private readonly PushMarketContext _dbContext;

        public PasswordResetLinkRepository(PushMarketContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(PasswordResetLink passwordResetLink)
        {
            _dbContext.PasswordResetLinks.Add(passwordResetLink);
        }

        public PasswordResetLink Retrieve(Guid guid)
        {
            return _dbContext.PasswordResetLinks.FirstOrDefault(x => x.ExternalGuid == guid);
        }
    }
}
