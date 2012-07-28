using System;
using Pleiades.Framework.Injection;
using Pleiades.Commerce.Domain.Concrete;

namespace Pleiades.Commerce.Domain
{
    public class DomainRegistration
    {
        public static void Register(IGenericBuilder builder)
        {
            builder.RegisterType<AggregateUserService>();
        }
    }
}
