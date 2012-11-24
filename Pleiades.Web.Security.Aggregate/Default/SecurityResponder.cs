using System;
using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Default
{
    /// <summary>
    /// Abstracts and enables composition of responses to Security Authorization attempts
    /// </summary>
    public class SecurityResponder : IHttpSecurityResponder
    {
        /// <summary>
        /// Based on the SecurityResponseCode
        /// </summary>
        public void Process(SecurityCode securityCode, AuthorizationContext filterContext)
        {
            if (securityCode == SecurityCode.Allowed)
                Allowed(filterContext);

            if (securityCode == SecurityCode.AccessDenied)
                AccessDenied(filterContext);

            if (securityCode == SecurityCode.AccessDeniedSolicitLogon)
                AccessDeniedSolicitLogon(filterContext);

            if (securityCode == SecurityCode.AccountDisabledNonPayment)
                AccountDisabledNonPayment(filterContext);

            if (securityCode == SecurityCode.AccessDeniedToAccountLevel)
                AccessDeniedToAccountLevel(filterContext);
        }


        protected virtual void Allowed(AuthorizationContext filterContext)
        {
            
        }

        /// <summary>
        /// Template Method for responding to AccessDenied
        /// </summary>
        protected virtual void AccessDenied(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            filterContext.Result = new HttpUnauthorizedResult();
        }

        /// <summary>
        /// Template Method for responding to AccessDeniedSolicitLogon.  
        /// Default functionality 302's to the FormsAuthentication.LoginUrl
        /// </summary>
        protected virtual void AccessDeniedSolicitLogon(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            filterContext.Result = new HttpUnauthorizedResult();
        }

        /// <summary>
        /// Template Method for responding to AccountDisabledNonPayment
        /// </summary>
        protected virtual void AccountDisabledNonPayment(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            filterContext.Result = new HttpUnauthorizedResult();
        }

        /// <summary>
        /// Template Method for responding to AccessDeniedToAccountLevel
        /// </summary>
        protected virtual void AccessDeniedToAccountLevel(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 302;
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}
