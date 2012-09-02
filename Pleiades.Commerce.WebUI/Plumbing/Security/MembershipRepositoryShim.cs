using System;
using Pleiades.Web.Security.Providers;
using Commerce.Persist;
using Commerce.Persist.Security;

namespace CommerceInitializer
{
    public class MembershipRepositoryShim
    {
        // ### BIG, BIG, TODO: wire this into Autofac so that with each ASP.NET MVC Request, it gets its own lifetime scope
        // *** FOLLOW-UP => you can't do that, son!

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