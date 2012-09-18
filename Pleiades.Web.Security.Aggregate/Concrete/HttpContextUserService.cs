using System.Web;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Concrete
{
    public class HttpContextUserService : IHttpContextUserService
    {
        public AggregateUser GetCurrentUserFromHttpContext()
        {
            return HttpContext.Current.Items["CurrentUserCache"] as AggregateUser;
        }

        public void PutCurrentUserInHttpContext(AggregateUser user)
        {
            HttpContext.Current.Items["CurrentUserCache"] = user;
        }
    }
}