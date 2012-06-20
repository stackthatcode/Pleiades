using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Pleiades.Framework.Data;
using Pleiades.Framework.Data.EF;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Persist
{
    public class MembershipRepository : EFGenericRepository<MembershipUser>, IMembershipRepository
    {
        public MembershipRepository(DbContext context) : base(context)
        {
        }

        public string ApplicationName { get; set; }

        public int UserIsOnlineTimeWindow { get; set; } 


        protected override IQueryable<MembershipUser> Data()
        {
            return base.Data().Where(x => x.ApplicationName == this.ApplicationName);
        }

        public MembershipUser GetUser(string username)
        {
            return this.FindFirstOrDefault(x => x.UserName == username);
        }

        public MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = this.GetUser(username);
            
            // TODO: add time check

            return user;
            // if (userIsOnline
        }

        public MembershipUser GetUserByProviderKey(object providerKey, bool userIsOnline)
        {
            throw new System.NotImplementedException();
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
            throw new NotFiniteNumberException();
        }

        public int GetNumberOfUsersOnline()
        {
            throw new NotFiniteNumberException();

            //PleiadesDB context = new PleiadesDB(ConnectionString);

            //return (from u in context.Users
            //        where u.ApplicationName == applicationName && u.LastActivityDate > compareTime
            //        select u).Distinct().Count();

        }

        private DateTime LastActivityCutoff()
        {
            var onlineSpan = new TimeSpan(0, this.UserIsOnlineTimeWindow, 0);
            var compareTime = DateTime.Now.Subtract(onlineSpan);
            return compareTime;
        }

        public IList<MembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public IList<MembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        
            //MembershipUserCollection membershipUsers = new MembershipUserCollection();
            //PleiadesDB context = new PleiadesDB(ConnectionString);
            //IQueryable<User> users = from u in context.Users
            //                         where u.Username.Contains(usernameToMatch) && u.ApplicationName == applicationName
            //                         orderby u.Username
            //                         select u;

            //totalRecords = users.Count();
            //if (users.Count() <= 0) 
            //    return membershipUsers;

            //foreach (User user in users.Skip(pageIndex * pageSize).Take(pageSize))
            //{
            //    membershipUsers.Add(GetMembershipUserFromPersitentObject(user));
            //}
            //
            //return membershipUsers;    
    
            //MembershipUserCollection membershipUsers = new MembershipUserCollection();
            //PleiadesDB context = new PleiadesDB(ConnectionString);
            //IQueryable<User> users = from u in context.Users
            //                         where u.Email.Contains(emailToMatch) && u.ApplicationName == applicationName
            //                         select u;
            //
            //totalRecords = users.Count();
            //if (users.Count() <= 0) 
            //    return membershipUsers;
            //
            //foreach (User user in users.Skip(pageIndex * pageSize).Take(pageSize))
            //{
            //    membershipUsers.Add(GetMembershipUserFromPersitentObject(user));
            //}
            //
            //return membershipUsers;    
    }
}
