using System.Linq;
using Commerce.Application.Database;
using Pleiades.Application.Data.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Application.Security
{
    public class MembershipWritableRepository : EFGenericRepository<PfMembershipUser>, IMembershipWritableRepository
    {
        public MembershipWritableRepository(PushMarketContext context)
            : base(context)
        {
        }


        void IMembershipWritableRepository.DeleteUser(string username, bool deleteAllRelatedData)
        {
            var user = this.FirstOrDefault(x => x.UserName == username);
            this.Delete(user);
        }

        PfMembershipUser IMembershipWritableRepository.GetUserByUserName(string username)
        {
            return this.Data().FirstOrDefault(x => x.UserName == username);
        }

        PfMembershipUser IMembershipWritableRepository.GetUserByEmail(string email)
        {
            return this.Data().FirstOrDefault(x => x.Email == email);
        }

        void IMembershipWritableRepository.AddUser(PfMembershipUser user)
        {
            this.Insert(user);
        }
    }
}
