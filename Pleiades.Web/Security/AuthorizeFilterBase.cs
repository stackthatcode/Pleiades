using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Web.Security
{
    /// <summary>
    /// Base class for deriving Pleiades Framework-powered ASP.NET MVC Authorization Attributes
    /// </summary>
    public abstract class AuthorizeFilterBase<T> : IAuthorizationFilter
            where T : ISecurityContext
    {    
        // TODO: can these be injected by MVC...?
        protected abstract Func<AuthorizationContext, T> BuildSecurityContext { get; }
        
        protected abstract Func<Step<T>> BuildAuthorizationExecution { get; }

        protected abstract Func<SecurityCodeFilterResponder> BuildSecurityCodeFilterResponder { get; }

        /// <summary>
        /// Overrides the ASP.NET MVC default authorization with Framework logic
        /// </summary>
        /// <param name="filterContext">ASP.NET MVC AuthorizationContext</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var authorizationProcess = this.BuildAuthorizationExecution();
            var context = this.BuildSecurityContext(filterContext);

            authorizationProcess.Execute(context);
            var responseCode = context.SecurityResponseCode;

            var responder = this.BuildSecurityCodeFilterResponder();
            responder.ProcessSecurityCode(responseCode, filterContext);
        }
    }
}