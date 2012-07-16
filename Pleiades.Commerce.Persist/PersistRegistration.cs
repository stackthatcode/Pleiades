using System;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Data.EF;
using Pleiades.Commerce.Persist.Users;

namespace Pleiades.Commerce.Persist
{
    public class PersistRegistration : IRegistration
    {
        public void Register(IGenericBuilder builder)
        {
            // Context and Unit of Work
            builder.RegisterType<PleiadesContext>();
            builder.RegisterType<EFUnitOfWork>();

            // Domain-serving Repositories
            builder.RegisterType<AggregateUserRepository>();
            builder.RegisterType<IdentityUserRepository>();
            builder.RegisterType<MembershipRepository>();

            // TODO: add the Products Repos
        }
    }
}
