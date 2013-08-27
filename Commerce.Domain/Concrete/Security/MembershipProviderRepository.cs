using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Application;
using Pleiades.Application.Data;
using Pleiades.Application.EF;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Commerce.Application.Database;

namespace Commerce.Application.Concrete.Security
{
    public class PfMembershipRepository : EFGenericRepository<PfMembershipUser>, IMembershipProviderRepository
    {
        // QUESTION: how to get the applicationName into the ReadOnlyRepository...?
        // ANSWER: Autofac Registration
        // TODO: either add database indexes, or craft a plan to eliminate ApplicationName -- don't need it.

        [Obsolete]
        public string ApplicationName { get; set; }

        public PfMembershipRepository(PushMarketContext context, string applicationName = "/")
            : base(context)
        {
            this.ApplicationName = applicationName;
        }

        protected override IQueryable<PfMembershipUser> Data()
        {
            return base.Data().Where(x => x.ApplicationName == this.ApplicationName);
        }

        public PfMembershipUser GetUser(string username)
        {
            return this.FirstOrDefault(x => x.UserName == username);
        }

        public PfMembershipUser GetUserByProviderKey(object providerKey)
        {
            return this.FirstOrDefault(x => x.ProviderUserKey == providerKey.ToString());
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

        public IList<PfMembershipUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = this.GetAll().Count();
            return this.Data().Page(pageIndex, pageSize).ToList();
        }

        public IList<PfMembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var results = 
                (from u in this.Data()
                 where u.UserName.Contains(usernameToMatch)
                 orderby u.UserName
                 select u);

            totalRecords = results.Count();
            return results.ToList();
        }

        public IList<PfMembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
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


        public IList<PfMembershipUser> FindUsersByEmail(string emailToMatch)
        {
            throw new NotImplementedException();
        }
    }
}
