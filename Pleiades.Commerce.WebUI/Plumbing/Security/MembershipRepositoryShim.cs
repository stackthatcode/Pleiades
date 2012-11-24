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
                // TODO: wire this so that the same Database Context per Request is passed around
                var _dbContext = new PleiadesContext();
                return new PfMembershipRepository(_dbContext, settings.ApplicationName);
            });
        }
    }
}