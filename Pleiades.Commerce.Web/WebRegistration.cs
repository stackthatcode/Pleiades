using System;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity;
using Pleiades.Framework.MembershipProvider;
using Pleiades.Framework.Web.Interface;
using Pleiades.Framework.Web.Security;
using Pleiades.Commerce.Web.Security.Concrete;
using Pleiades.Commerce.Web.Security.Execution.Abstract;
using Pleiades.Commerce.Web.Security.Execution.Composites;
using Pleiades.Commerce.Web.Security.Execution.Steps;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web
{
    public class WebRegistration
    {
        public static void Register(IGenericBuilder builder)
        {
            // TODO: explicitly register Identity stuff here...?
            // TODO: explicitly register Membership stuff here....?

            // Pleiades.Framework.Identity
            IdentityRegistration.RegisterConcrete(builder);
            IdentityRegistration.RegisterOwnerAuthorizationByContext<OwnerAuthorizationContextBase>(builder);
            IdentityRegistration.RegisterSystemAuthorizationByContext<SystemAuthorizationContextBase>(builder);

            // Pleiades.Framework.MembershipProvider
            MembershipRegistration.Register(builder);

            // Concrete
            builder.RegisterTypeAs<SecurityCodeResponder, ISecurityCodeFilterResponder>();

            // Composites
            builder.RegisterType<ChangeUserPasswordComposite>();
            builder.RegisterType<GetUserFromContextStep>();

            // Steps
            builder.RegisterType<AuthenticateUserByRoleStep>();
            builder.RegisterType<ChangeUserPasswordStep>();
            builder.RegisterType<GetUserFromContextStep>();
            builder.RegisterType<LogoutStep>();
        }
    }
}