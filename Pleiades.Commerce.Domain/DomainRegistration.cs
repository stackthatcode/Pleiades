using System;
using Pleiades.Framework.Injection;
using Pleiades.Commerce.Domain.Concrete;

namespace Pleiades.Commerce.Domain
{
    public class DomainRegistration : IRegistration
    {
        public void Register(IGenericBuilder builder)
        {
            builder.RegisterType<AggregateUserService>();
        }
    }
}
