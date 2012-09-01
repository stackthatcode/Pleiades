using System.Web.Mvc;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Concrete
{
    public class DefaultAuthorizationContextBuilder : ISystemAuthorizationContextBuilder
    {
        public virtual ISystemAuthorizationContext Build(AuthorizationContext filterContext)
        {
            return new DefaultAuthorizationContext();
        }
    }
}