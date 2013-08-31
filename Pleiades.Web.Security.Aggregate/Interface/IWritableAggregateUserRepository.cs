using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IWritableAggregateUserRepository
    {
        AggregateUser RetrieveByMembershipUserName(string username);
        AggregateUser RetrieveById(int aggregateUserId);
        AggregateUser Add(AggregateUser aggregateUser);
        void Delete(int id);
    }
}