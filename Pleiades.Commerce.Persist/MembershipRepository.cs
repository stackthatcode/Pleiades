using System.Collections.Generic;
using System.Data.Entity;
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

        public static void TestOfTests()
        {
            IGenericRepository<MembershipUser> repository = new EFGenericRepository<MembershipUser>(null);
        }


        public string ApplicationName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public MembershipUser GetUser(string username)
        {
            throw new System.NotImplementedException();
        }

        public MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new System.NotImplementedException();
        }

        public MembershipUser GetUserByProviderKey(object providerKey, bool userIsOnline)
        {
            throw new System.NotImplementedException();
        }

        public string GetUserNameByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IList<MembershipUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public int GetNumberOfUsersOnline()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IList<MembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IList<MembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        

        //IQueryable<User> users = from u in context.Users
        //                         where u.Username == username && u.ApplicationName == applicationName
        //                         select u;

        
        //PleiadesDB context = new PleiadesDB(ConnectionString);
        //IQueryable<User> users = from u in context.Users
        //                         where u.Email == email && u.ApplicationName == applicationName
        //                         select u;

        //if (users.Count() != 1) 
        //    return string.Empty;

        //User user = users.First();
        //return user != null ? user.Username : string.Empty;

            //        TimeSpan onlineSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
            //DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            //PleiadesDB context = new PleiadesDB(ConnectionString);

            //return (from u in context.Users
            //        where u.ApplicationName == applicationName && u.LastActivityDate > compareTime
            //        select u).Distinct().Count();



            //        MembershipUserCollection membershipUsers = new MembershipUserCollection();
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
