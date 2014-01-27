using System;
using System.Linq;
using Commerce.Application.Database;
using Pleiades.App.Data;

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
            foreach (var oldlink in _dbContext.PasswordResetLinks
                        .Where(x => x.MembershipUserName == passwordResetLink.MembershipUserName))
            {
                _dbContext.Delete(oldlink);
            }
            _dbContext.PasswordResetLinks.Add(passwordResetLink);
        }

        public PasswordResetLink Retrieve(Guid guid)
        {
            return _dbContext.PasswordResetLinks.FirstOrDefault(x => x.ExternalGuid == guid);
        }
    }
}
