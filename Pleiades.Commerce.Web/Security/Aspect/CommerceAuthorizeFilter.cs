using System;
using System.Web.Mvc;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Security;
using Pleiades.Framework.Web.Security;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security
{
    public abstract class CommerceAuthorizeAttribute : AuthorizeFilterBase<CommerceSecurityContext>
    {
        public IContainer Container { get; set; }

        #region Attribute Properties
        /// <summary>
        /// The AuthorizationZone specifies 
        /// </summary>
        public AuthorizationZone AuthorizationZone { get; set; }

        /// <summary>
        /// The Account Level Restriction identifies the *minimum* Account Level which is allowed authorization
        /// </summary>
        public AccountLevel AccountLevelRestriction { get; set; }

        /// <summary>
        /// Identifies if this is a Payment Area, so delinquent accounts can take care of business
        /// </summary> 
        public bool PaymentArea { get; set; }
        #endregion

        public CommerceAuthorizeAttribute(IContainer container)
        {
            this.Container = container;
        }

        protected override CommerceSecurityContext BuildSecurityContext(AuthorizationContext filterContext)
        {
            return new CommerceSecurityContext
            {
                HttpContext = filterContext.HttpContext,
                IdentityRequirements = new IdentityRequirements
                {
                    AuthorizationZone = this.AuthorizationZone,
                    AccountLevelRestriction = this.AccountLevelRestriction,
                    PaymentArea = this.PaymentArea,
                },
            };
        }

        protected override Func<Step<CommerceSecurityContext>> BuildAuthorizationStep
        {
            get
            {
                return () => this.Container.Resolve<AuthorizationStepComposite>();
            }
        }

        protected override SecurityCodeProcessorBase BuildSecurityCodeProcessor
        {
            get 
            {
                return new SecurityCodeProcessorBase();
            }
        }
    }
}
