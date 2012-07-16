using System;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity;
using Pleiades.Framework.MembershipProvider;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Execution.NonPublic;
using Pleiades.Commerce.Web.Security.Factories;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web
{
    public class WebRegistration : IRegistration
    {
        public void Register(IGenericBuilder builder)
        {
            // Pleiades.Framework.Identity
            var identityBuilder = new IdentityRegistration(builder);
            identityBuilder.RegisterConcrete();
            identityBuilder.RegisterOwnerAuthorization<OwnerAuthorizationContext>();
            identityBuilder.RegisterSystemAuthorization<SystemAuthorizationContextBase>();

            // Pleiades.Framework.MembershipProvider
            var membershipBuilder = new MembershipRegistration(builder);
            membershipBuilder.Register();

            // Pleiades.Commerce.Web
            builder.RegisterType<ChangeUserPasswordStep>();
            builder.RegisterType<AuthorizeFromFilterComposite>();
            builder.RegisterType<GetUserFromFilterContextStep>();
            builder.RegisterType<ChangeUserPasswordStepFactory>();

            // Pleiades.Framework.Web
        }
    }
}
