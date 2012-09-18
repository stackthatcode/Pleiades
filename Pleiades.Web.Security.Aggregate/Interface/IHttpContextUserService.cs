using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IHttpContextUserService
    {
        AggregateUser GetCurrentUserFromHttpContext();
        void PutCurrentUserInHttpContext(AggregateUser user);
    }
}