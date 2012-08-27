using System.Web.Mvc;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface ISystemAuthContextBuilder
    {
        SystemAuthorizationContextBase Build(AuthorizationContext filterContext);
    }
}