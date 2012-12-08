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
    public class PfMembershipRepository : EFGenericRepository<MembershipUser>, IMembershipProviderRepository
    {
        // QUESTION: how to get the applicationName into the Repository...?
        // ANSWER: Autofac Registration
        // TODO: either add database indexes, or craft a plan to eliminate ApplicationName -- don't need it.

        [Obsolete]
        public string ApplicationName { get; set; }

        public PfMembershipRepository(DbContext context, string applicationName = "/")
            : base(context)
        {
            this.ApplicationName = applicationName;
        }

        protected override IQueryable<MembershipUser> TrackableData()
        {
            return base.TrackableData().Where(x => x.ApplicationName == this.ApplicationName);
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
            var user = this.TrackableData()
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
            return this.TrackableData().Page(pageIndex, pageSize).ToList();
        }

        public IList<MembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var results = 
                (from u in this.TrackableData()
                 where u.UserName.Contains(usernameToMatch)
                 orderby u.UserName
                 select u);

            totalRecords = results.Count();
            return results.ToList();
        }

        public IList<MembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var results =
                (from u in this.TrackableData()
                 where u.Email.Contains(emailToMatch)
                 orderby u.Email
                 select u);

            totalRecords = results.Count();
            return results.ToList();
        }

        public int GetNumberOfUsersOnline(TimeSpan userIsOnlineTimeWindow)
        {
            return (from u in this.TrackableData()
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
