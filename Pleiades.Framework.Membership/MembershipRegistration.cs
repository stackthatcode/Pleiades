using System;
using Pleiades.Framework.Injection;
using Pleiades.Framework.MembershipProvider.Concrete;

namespace Pleiades.Framework.MembershipProvider
{
    public class MembershipRegistration
    {
        public static void Register(IGenericBuilder builder)
        {
            builder.RegisterType<FormsAuthenticationService>();
            builder.RegisterType<MembershipService>();
        }
    }
}