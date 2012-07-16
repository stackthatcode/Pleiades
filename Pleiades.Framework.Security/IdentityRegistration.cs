using System;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Concrete;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.Identity.Factories;
using Pleiades.Framework.Identity.Interface;

namespace Pleiades.Framework.Identity
{
    public class IdentityRegistration
    {
        IGenericBuilder Builder;

        public IdentityRegistration(IGenericBuilder builder)
        {
            this.Builder = builder;
        }

        public void RegisterConcrete()
        {
            this.Builder.RegisterType<IdentityUserService>();
        }

        public void RegisterSystemAuthorization<TContext>() where TContext : ISystemAuthorizationContext
        {
            this.Builder.RegisterType<AccountLevelAuthorizationStep<TContext>>();
            this.Builder.RegisterType<AccountStatusAuthorizationStep<TContext>>();
            this.Builder.RegisterType<RoleAuthorizationStep<TContext>>();
        }

        public void RegisterOwnerAuthorization<TContext>() where TContext : IOwnerAuthorizationContext
        {
            this.Builder.RegisterType<SimpleOwnerAuthorizationStep<TContext>>();
            this.Builder.RegisterType<OwnerAuthorizedStepFactory<TContext>>();
        }
    }
}