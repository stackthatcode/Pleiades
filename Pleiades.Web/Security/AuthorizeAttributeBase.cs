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
    public abstract class AuthorizeAttributeBase : AuthorizeAttribute
    {
        public abstract Step<ISecurityContext> AuthorizationStep { get; }

        public abstract ISecurityContext BuildSecurityContext(AuthorizationContext filterContext);

        public abstract SecurityCodeProcessorBase SecurityCodeProcessor { get; }

        /// <summary>
        /// Overrides the ASP.NET MVC default authorization with Framework logic
        /// </summary>
        /// <param name="filterContext">ASP.NET MVC AuthorizationContext</param>
        public override sealed void OnAuthorization(AuthorizationContext filterContext)
        {
            var context = this.BuildSecurityContext(filterContext);

            this.AuthorizationStep.Execute(context);

            this.SecurityCodeProcessor.ProcessSecurityCode(context.SecurityResponseCode, filterContext);
        }
    }
}