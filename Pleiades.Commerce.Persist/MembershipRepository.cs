﻿using System;
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

        public int GetNumberOfUsersOnline()
        {
            return (from u in this.Data()
                    where u.LastActivityDate > LastActivityCutoff()
                    select u).Distinct().Count();

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
                 where u.UserName.Contains(emailToMatch)
                 orderby u.UserName
                 select u);

            totalRecords = results.Count();
            return results.ToList();
        }

        private DateTime LastActivityCutoff()
        {
            var onlineSpan = new TimeSpan(0, this.UserIsOnlineTimeWindow, 0);
            var compareTime = DateTime.Now.Subtract(onlineSpan);
            return compareTime;
        }
    }
}
