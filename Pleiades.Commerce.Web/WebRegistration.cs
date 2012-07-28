using System;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity;
using Pleiades.Framework.MembershipProvider;
using Pleiades.Framework.Web.Interface;
using Pleiades.Framework.Web.Security;
using Pleiades.Commerce.Web.Security.Concrete;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Execution.NonPublic;
using Pleiades.Commerce.Web.Security.Factories;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web
{
    public class WebRegistration
    {
        public static void Register(IGenericBuilder builder)
        {
            // Pleiades.Framework.Identity
            IdentityRegistration.Register(builder);
            IdentityRegistration.RegisterOwnerAuthorization<OwnerAuthorizationContext>(builder);
            IdentityRegistration.RegisterSystemAuthorization<SystemAuthorizationContextBase>(builder);

            // Pleiades.Framework.MembershipProvider
            MembershipRegistration.Register(builder);

            // Pleiades.Commerce.Web
            builder.RegisterType<ChangeUserPasswordStep>();
            builder.RegisterType<AuthorizeFromFilterComposite>();
            builder.RegisterType<GetUserFromFilterContextStep>();
            builder.RegisterType<ChangeUserPasswordStepFactory>();

            // Pleiades.Framework.Web
            builder.RegisterTypeAs<CommerceSecurityCodeResponder, ISecurityCodeFilterResponder>();
        }
    }
}