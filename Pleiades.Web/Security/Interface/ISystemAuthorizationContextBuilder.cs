using System.Web.Mvc;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Execution.Context;

namespace Pleiades.Web.Security.Interface
{
    public interface ISystemAuthorizationContextBuilder
    {
        ISystemAuthorizationContext Build(AuthorizationContext filterContext);
    }
}