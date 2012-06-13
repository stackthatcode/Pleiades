using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Framework.Data;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Providers
{
    /// <summary>
    /// Contract for Persistence Layer operations needed by the Membership Provider
    /// </summary>
    public interface IMembershipRepository : IGenericRepository<MembershipUser>
    {
        string ApplicationName { get; set; }
        MembershipUser GetUser(string username);
        MembershipUser GetUser(string username, bool userIsOnline);
        MembershipUser GetUserByProviderKey(object providerKey, bool userIsOnline);
        string GetUserNameByEmail(string email);
        bool DeleteUser(string username, bool deleteAllRelatedData);
        IList<MembershipUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);
        int GetNumberOfUsersOnline();
        IList<MembershipUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);
        IList<MembershipUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);


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