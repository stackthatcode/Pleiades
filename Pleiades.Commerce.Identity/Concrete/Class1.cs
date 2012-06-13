using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Identity
{
    public class Class1
    {

        /// <summary>
        /// Retrieve Domain User by Email Address
        /// </summary>
        public Model.DomainUser RetrieveUserByEmail(string emailaddr)
        {
            var username = Membership.GetUserNameByEmail(emailaddr);
            return RetrieveUserByMembershipUserName(username);
        }

        /// <summary>
        /// Retrieve Domain User by UserName - actually UserCode
        /// </summary>        
        public Model.DomainUser RetrieveUserByMembershipUserName(string username)
        {
            var pleiadesDB = new PleiadesDB();
            var domainUserEntity = pleiadesDB.DomainUsers.FirstOrDefault(x => x.User.Username == username);

            if (domainUserEntity == null)
                return null;

            return RetrieveHelper(username, domainUserEntity);
        }

        /// <summary>
        /// Retrieve All method with built-in Paging and User Role filtering
        /// </summary>
        public IEnumerable<DomainUserCondensed> RetreiveAll(int pageNumber, int pageSize, List<UserRole> role)
        {
            var listOfRoleStrings = role.Select(x => x.ToString()).ToList();

            var pleiadesDB = new PleiadesDB();
            var records =
                from user in pleiadesDB.Users
                join domainUser in pleiadesDB.DomainUsers
                    on user.Id equals domainUser.MembershipUserId
                where listOfRoleStrings.Contains(domainUser.UserRole)
                select new DomainUserCondensed
                {
                    DomainUser = domainUser,
                    MembershipUser = user,
                };

            return RetrieveUserListItemOutput(records, pageNumber, pageSize);
        }

        /// <summary>
        /// Retrieve filtered by Paging, User Role and Email Address
        /// </summary>
        public IEnumerable<DomainUserCondensed> RetreiveByLikeEmail(
                string emailAddressToMatch, int pageNumber, int pageSize, List<UserRole> role)
        {
            var listOfRoleStrings = role.Select(x => x.ToString()).ToList();
            var pleiadesDB = new PleiadesDB();
            var records =
                from user in pleiadesDB.Users
                join domainUser in pleiadesDB.DomainUsers on user.Id equals domainUser.MembershipUserId
                where user.Email.Contains(emailAddressToMatch) && listOfRoleStrings.Contains(domainUser.UserRole)
                select new DomainUserCondensed
                {
                    DomainUser = domainUser,
                    MembershipUser = user,
                };

            return RetrieveUserListItemOutput(records, pageNumber, pageSize);
        }

        private IEnumerable<DomainUserCondensed> RetrieveUserListItemOutput(
                IQueryable<DomainUserCondensed> records, int pageNumber, int pageSize)
        {
            var count = records.Count();
            return records.Page<DomainUserCondensed>(pageNumber, pageSize);
        }
    }
}
