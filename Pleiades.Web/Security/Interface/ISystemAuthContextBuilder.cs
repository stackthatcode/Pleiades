using System.Web.Mvc;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Interface
{
    public interface ISystemAuthContextBuilder
    {
        SystemAuthorizationContextBase Build(AuthorizationContext filterContext);
    }
}