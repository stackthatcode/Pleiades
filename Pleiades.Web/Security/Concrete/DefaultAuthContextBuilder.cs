using System.Web.Mvc;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Concrete
{
    public class DefaultAuthContextBuilder : ISystemAuthContextBuilder
    {
        public virtual SystemAuthorizationContextBase Build(AuthorizationContext filterContext)
        {
            return new SystemAuthorizationContextBase(filterContext.HttpContext);
        }
    }
}