using System.Linq;
using Commerce.Application.Database;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Application.Concrete.Security
{
    public class AggregateWriteableRepository : IWritableAggregateUserRepository
    {
        private readonly PushMarketContext _context;

        public AggregateWriteableRepository(PushMarketContext context)
        {
            _context = context;
        }

        public AggregateUser RetrieveByMembershipUserName(string username)
        {
            return _context.AggregateUsers.FirstOrDefault(x => x.Membership.UserName == username);
        }

        public AggregateUser RetrieveById(int aggregateUserId)
        {
            return _context.AggregateUsers.FirstOrDefault(x => x.ID == aggregateUserId);
        }


        public AggregateUser Add(AggregateUser aggregateUser)
        {
            return _context.AggregateUsers.Add(aggregateUser);
        }

        public void Delete(int id)
        {
            var user = _context.AggregateUsers.First(x => x.ID == id);
            _context.AggregateUsers.Remove(user);
        }
    }
}
