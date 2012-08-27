using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Persist.Security
{
    public class MembershipRepository : EFGenericRepository<MembershipUser>, IMembershipRepository
    {
        public MembershipRepository(DbContext context) : base(context)
        {
        }

        public string ApplicationName { get; set; }

        protected override IQueryable<MembershipUser> Data()
        {
            return base.Data().Where(x => x.ApplicationName == this.ApplicationName);
        }

        public MembershipUser GetUser(string username)
        {
            return this.FindFirstOrDefault(x => x.UserName == username);
        }

        public MembershipUser GetUserByProviderKey(object providerKey)
        {
            return this.FindFirstOrDefault(x => x.ProviderUserKey == providerKey.ToString());
        }

        public string GetUserNameByEmail(string email)
        {
            var user = this.Data()
                .FirstOrDefault(x => x.Email == email);

            return user == null ? string.Empty : user.UserName;
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var user = this.GetUser(username);
            this.Delete(user);
            return true;
        }

        public IList<MembershipUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = this.Count();
            return this.Data().Page(pageIndex, pageSize).ToList();
        }

        public IList<MembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var results = 
                (from u in this.Data()
                 where u.UserName.Contains(usernameToMatch)
                 orderby u.UserName
                 select u);

            totalRecords = results.Count();
            return results.ToList();
        }

        public IList<MembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var results =
                (from u in this.Data()
                 where u.Email.Contains(emailToMatch)
                 orderby u.Email
                 select u);

            totalRecords = results.Count();
            return results.ToList();
        }

        public int GetNumberOfUsersOnline(TimeSpan userIsOnlineTimeWindow)
        {
            return (from u in this.Data()
                    where u.LastActivityDate > LastActivityCutoff(userIsOnlineTimeWindow)
                    select u).Distinct().Count();
        }

        private DateTime LastActivityCutoff(TimeSpan userIsOnlineTimeWindow)
        {
            var compareTime = DateTime.Now.Subtract(userIsOnlineTimeWindow);
            return compareTime;
        }
    }
}
