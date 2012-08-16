using System.Web.Mvc;

namespace Pleiades.Framework.Web.Security.Aspect
{
    public class PleiadesAuthorizeRule 
    {
        public virtual bool MustAuthorize(AuthorizationContext filterContext)
        {
            return true;
        }
    }
}