using System;
using Pleiades.Web.Security.Providers;
using Commerce.Persist;
using Commerce.Persist.Security;

namespace CommerceInitializer
{
    public class PfMembershipShimInit
    {
        // BIG, BIG, TODO: wire this into Autofac so that with each ASP.NET MVC Request, it gets its own lifetime scope

        public static void SetFactory()
        {
            PfMembershipRepositoryShim.RepositoryFactory =
            () =>
            {
                var _dbContext = new PleiadesContext();
                var _repository = new MembershipRepository(_dbContext);
                _repository.ApplicationName = System.Web.Security.Membership.ApplicationName;
                return _repository;
            };
        }
    }
}