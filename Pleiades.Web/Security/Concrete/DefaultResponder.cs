using System;
using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Security;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Concrete
{
    /// <summary>
    /// Abstracts and enables composition of responses to Security Authorization attempts
    /// </summary>
    public class DefaultResponder : IPostbackSecurityResponder
    {
        /// <summary>
        /// Based on the SecurityResponseCode
        /// </summary>
        public void Execute(SecurityResponseCode securityResponseCode, AuthorizationContext filterContext)
        {
            if (securityResponseCode == SecurityResponseCode.Allowed)
                Allowed(filterContext);

            if (securityResponseCode == SecurityResponseCode.AccessDenied)
                AccessDenied(filterContext);

            if (securityResponseCode == SecurityResponseCode.AccessDeniedSolicitLogon)
                AccessDeniedSolicitLogon(filterContext);

            if (securityResponseCode == SecurityResponseCode.AccountDisabledNonPayment)
                AccountDisabledNonPayment(filterContext);

            if (securityResponseCode == SecurityResponseCode.AccessDeniedToAccountLevel)
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
