using System;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Concrete;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.Identity.Interface;

namespace Pleiades.Framework.Identity
{
    public class IdentityRegistration
    {
        public static void RegisterConcrete(IGenericBuilder builder)
        {
            builder.RegisterType<IdentityUserService>();
        }

        public static void RegisterSystemAuthorizationByContext<TContext>(IGenericBuilder builder) 
                where TContext : ISystemAuthorizationContext
        {
            builder.RegisterType<AccountLevelAuthorizationStep<TContext>>();
            builder.RegisterType<AccountStatusAuthorizationStep<TContext>>();
            builder.RegisterType<RoleAuthorizationStep<TContext>>();
        }

        public static void RegisterOwnerAuthorizationByContext<TContext>(IGenericBuilder builder) 
                where TContext : IOwnerAuthorizationContext
        {
            builder.RegisterType<SimpleOwnerAuthorizationStep<TContext>>();
        }
    }
}