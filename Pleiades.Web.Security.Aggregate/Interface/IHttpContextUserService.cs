using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IHttpContextUserService
    {
        AggregateUser Get();
        void Put(AggregateUser user);
    }
}