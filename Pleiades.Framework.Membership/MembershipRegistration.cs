using System;
using Pleiades.Framework.Injection;
using Pleiades.Framework.MembershipProvider.Concrete;

namespace Pleiades.Framework.MembershipProvider
{
    public class MembershipRegistration
    {
        IGenericBuilder Builder;

        public MembershipRegistration(IGenericBuilder builder)
        {
            this.Builder = builder;
        }

        public void Register()
        {
            this.Builder.RegisterType<FormsAuthenticationService>();
            this.Builder.RegisterType<MembershipService>();
        }
    }
}