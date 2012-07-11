using System.Web.Mvc;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Web.Interface
{
    interface ISecurityCodeFilterResponder
    {
        void ProcessSecurityCode(SecurityResponseCode securityResponseCode, AuthorizationContext filterContext);
    }
}