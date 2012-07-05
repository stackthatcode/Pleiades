using System;
using System.Web.Mvc;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Web.Security;
using Pleiades.Commerce.Web.Security.Execution.Authorization;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security
{
    public abstract class CommerceAuthorizeAttribute : AuthorizeFilterBase<AggrUserAuthContext>
    {
        // Commentary -- it's on consumers to decide how to do this


        public IContainer Container { get; set; }

        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }


        public CommerceAuthorizeAttribute(IContainer container)
        {
            this.Container = container;
        }


        protected override Func<AuthorizationContext, AggrUserAuthContext> BuildSecurityContext
        {
            get
            {
                return (authcontext) =>
                    new AggrUserAuthContext
                    {
                        HttpContext = authcontext.HttpContext,
                        IdentityRequirements = new IdentityRequirements
                        {
                            AuthorizationZone = this.AuthorizationZone,
                            AccountLevelRestriction = this.AccountLevelRestriction,
                            PaymentArea = this.IsPaymentArea,
                        },
                    };
            }
        }

        protected override Func<Step<AggrUserAuthContext>> BuildAuthorizationExecution
        {
            get
            {
                return () => this.Container.Resolve<AuthAggrUserFromHttpContextStep>();
            }
        }

        protected override Func<SecurityCodeFilterResponder> BuildSecurityCodeFilterResponder
        {
            get
            {
                return () => new SecurityCodeFilterResponder();
            }
        }
    }
}
