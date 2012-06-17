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
        protected abstract Step<T> BuildAuthorizationStep { get; }

        protected abstract T BuildSecurityContext(AuthorizationContext filterContext);

        protected abstract SecurityCodeProcessorBase BuildSecurityCodeProcessor { get; }

        /// <summary>
        /// Overrides the ASP.NET MVC default authorization with Framework logic
        /// </summary>
        /// <param name="filterContext">ASP.NET MVC AuthorizationContext</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var context = this.BuildSecurityContext(filterContext);

            this.BuildAuthorizationStep.Execute(context);

            this.BuildSecurityCodeProcessor.ProcessSecurityCode(context.SecurityResponseCode, filterContext);
        }
    }
}