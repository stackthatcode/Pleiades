using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Web.Security.Concrete;

namespace Pleiades.Web.Security.Attributes
{
    /// <summary>
    /// Base class for deriving Pleiades Framework-powered ASP.NET MVC Authorization Attributes
    /// </summary>
    public class AuthorizeAttributeBase : AuthorizeAttribute
    {
        // Service Dependencies
        public IHttpContextUserService HttpContextUserService { get; set; }
        public IDomainUserService DomainUserService { get; set; }
        public ISystemAuthorizationService SystemAuthorizationService { get; set; }


        #region Security Requirements Context Properties
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

        /// <summary>
        /// Local property extracts Attribute Properties
        /// </summary>
        protected SecurityRequirementsContext SecurityRequirementsContext
        {
            get
            {
                return new SecurityRequirementsContext
                {
                    AuthorizationZone = AuthorizationZone,
                    AccountLevelRestriction = AccountLevelRestriction,
                    PaymentArea = PaymentArea,
                };
            }
        }
        #endregion


        /// <summary>
        /// ctor
        /// </summary>
        public AuthorizeAttributeBase()
        {
            // TODO: wire these so the DI framework can inject them
            this.HttpContextUserService = new HttpContextUserService();
            this.DomainUserService = new DomainUserService();
            this.SystemAuthorizationService = new SystemAuthService();
        }

        /// <summary>
        /// Overrides the ASP.NET MVC default authorization with Framework logic
        /// </summary>
        /// <param name="filterContext">ASP.NET MVC AuthorizationContext</param>
        public override sealed void OnAuthorization(AuthorizationContext filterContext)
        {
            // *** TODO: is there some way to attach this object to the HttpContext?  Put it in a Session? *** //

            // Create a DomainUser object based on the HttpContext
            var domainUser = this.HttpContextUserService.RetrieveUserFromHttpContext(filterContext.HttpContext);

            // Execute the Authorization Service 
            var authcode = this.SystemAuthorizationService.Authorize(domainUser, this.SecurityRequirementsContext);

            // Process the resulting Auth Code
            this.ProcessSecurityCode(authcode, filterContext);
        }

        /// <summary>
        /// Based on the SecurityResponseCode
        /// </summary>
        protected void ProcessSecurityCode(SecurityResponseCode securityResponseCode, AuthorizationContext filterContext)
        {
            if (securityResponseCode == SecurityResponseCode.Allowed)
                return;

            if (securityResponseCode == SecurityResponseCode.AccessDenied)
                AccessDenied(filterContext);

            if (securityResponseCode == SecurityResponseCode.AccessDeniedSolicitLogon)
                AccessDeniedSolicitLogon(filterContext);

            if (securityResponseCode == SecurityResponseCode.AccountDisabledNonPayment)
                AccountDisabledNonPayment(filterContext);

            if (securityResponseCode == SecurityResponseCode.AccessDeniedToAccountLevel)
                AccessDeniedToAccountLevel(filterContext);
        }


        /// <summary>
        /// Template Method for responding to AccessDenied
        /// </summary>
        public virtual void AccessDenied(System.Web.Mvc.AuthorizationContext filterContext)
        {
            throw new Exception("User attempted to access unauthorized resource");
        }

        /// <summary>
        /// Template Method for responding to AccessDeniedSolicitLogon
        /// </summary>
        public virtual void AccessDeniedSolicitLogon(System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(
                FormsAuthentication.LoginUrl + "?returnUrl=" +
                    filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));

            filterContext.HttpContext.Response.StatusCode = 302;
            filterContext.Result = new HttpUnauthorizedResult();
        }

        /// <summary>
        /// Template Method for responding to AccountDisabledNonPayment
        /// </summary>
        public virtual void AccountDisabledNonPayment(System.Web.Mvc.AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Template Method for responding to AccessDeniedToAccountLevel
        /// </summary>
        public virtual void AccessDeniedToAccountLevel(System.Web.Mvc.AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}