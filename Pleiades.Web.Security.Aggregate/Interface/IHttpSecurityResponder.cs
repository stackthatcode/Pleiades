using System.Web.Mvc;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Interface for responding to SecurityResponseCode's with HTTP postback
    /// </summary>
    public interface IHttpSecurityResponder
    {
        void Process(SecurityCode securityResponseCode, AuthorizationContext filterContext);
    }
}