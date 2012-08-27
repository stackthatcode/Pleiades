using System.Web.Mvc;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Concrete
{
    public class DefaultAuthContextBuilder : ISystemAuthContextBuilder
    {
        public virtual SystemAuthorizationContextBase Build(AuthorizationContext filterContext)
        {
            return new SystemAuthorizationContextBase(filterContext.HttpContext);
        }
    }
}