using System;
using Pleiades.Framework.MembershipProvider.Providers;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.Persist.Users;
namespace Pleiades.Commerce.Initializer
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