using System.Web.Mvc;
using Pleiades.Security;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Interface for responding to SecurityResponseCode's with HTTP postback
    /// </summary>
    public interface IPostbackSecurityResponder
    {
        void Execute(SecurityResponseCode securityResponseCode, AuthorizationContext filterContext);
    }
}