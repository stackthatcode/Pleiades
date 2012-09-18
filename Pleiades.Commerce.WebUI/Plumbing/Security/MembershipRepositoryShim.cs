using System;
using Pleiades.Web.Security.Providers;
using Commerce.Persist;
using Commerce.Persist.Security;

namespace CommerceInitializer
{
    public class MembershipRepositoryShim
    {
        public static void SetFactory()
        {
            PfMembershipRepositoryBroker.Register((settings) =>
            {
                var _dbContext = new PleiadesContext();
                return new PfMembershipRepository(_dbContext, settings.ApplicationName);
            });
        }
    }
}