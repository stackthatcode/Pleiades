using System;
using System.Collections.Generic;
using System.Linq;

namespace Pleiades.Commerce.Identity.Interface
{
    public class AggregateRootUserRepository : 
    {
        public const int MaximumNumberOfAdmins = 3;



        /// <summary>
        /// TODO: separate this capability and create a standalone applet that handles this - make it part of 
        /// ... the deployment process
        /// 
        /// SOLUTION: create the UserBootstrapService
        /// 
        /// Ensures the Root User Account Exists
        /// </summary>
        // TODO: replace this with encrypted values stored in the Assembly manifest - or do the deploy thingy
        private const string defaultCode = "#3#3#3#3";
        private const string defaultEmail = "aleksjones@gmail.com";
        private const string defaultQuestion = "Dad's name";
        private const string defaultAnswer = "Donald";
 

        //public void Initialize()
        //{
        //    if (this.GetUserCountByRole(UserRole.Root) > 0)
        //    {
        //        return;
        //    }

        //    var user = this.Create(
        //        new CreateNewDomainUserRequest()
        //        {
        //            Password = defaultCode,
        //            Email = defaultEmail,
        //            PasswordQuestion = defaultQuestion,
        //            PasswordAnswer = defaultAnswer,
        //            IsApproved = true,
        //            AccountStatus = Model.AccountStatus.Active,
        //            UserRole = UserRole.Root,
        //            AccountLevel = AccountLevel.Gold,
        //            FirstName = "Aleks",
        //            LastName = "Jones"
        //        });

        //    if (user == null)
        //    {
        //        throw new Exception("Unable to create the default Root User");
        //    }
        //}
        
        /// <summary>
        /// DEPRECATED!
        /// 
        /// Worker method gets Users by User Name 
        /// </summary>
        //private Model.DomainUser RetrieveHelper(string membershipUserName, Data.DomainUser domainUserEntity)
        //{
        //    var domainUser = new Model.DomainUser()
        //    {
        //        DomainUserId = domainUserEntity.DomainUserId,
        //        UserRole = domainUserEntity.UserRole.StringToEnum<UserRole>(),
        //        AccountLevel = (Model.AccountLevel)domainUserEntity.AccountLevel,
        //        AccountStatus = (Model.AccountStatus)domainUserEntity.AccountStatus,
        //        FirstName = domainUserEntity.FirstName,
        //        LastName = domainUserEntity.LastName,
        //        LastModified = domainUserEntity.LastModified,
        //        CreationDate = domainUserEntity.CreationDate,
        //    };

        //    return domainUser;
        //}


        /// <summary>
        /// Retrieve Identity User by Email Address
        /// </summary>
        public Model.DomainUser RetrieveUserByEmail(string emailaddr)
        {
            var username = Membership.GetUserNameByEmail(emailaddr);
            return RetrieveUserByMembershipUserName(username);
        }

        /// <summary>
        /// Retrieve Identity User by UserName - actually UserCode
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
        /// DEPRECATED
        /// 
        /// Worker method gets Users by User Name
        /// </summary>
        //private Model.DomainUser RetrieveHelper(string membershipUserName, Data.DomainUser domainUserEntity)
        //{
        //    var domainUser = new Model.DomainUser()
        //    {
        //        DomainUserId = domainUserEntity.DomainUserId,
        //        UserRole = domainUserEntity.UserRole.StringToEnum<UserRole>(),
        //        AccountLevel = (Model.AccountLevel)domainUserEntity.AccountLevel,
        //        AccountStatus = (Model.AccountStatus)domainUserEntity.AccountStatus,
        //        FirstName = domainUserEntity.FirstName,
        //        LastName = domainUserEntity.LastName,
        //        LastModified = domainUserEntity.LastModified,
        //        CreationDate = domainUserEntity.CreationDate,
        //    };

        //    // Lil experiment
        //    // var domainUser2 = new Model.DomainUser();
        //    // domainUserEntity.AutoMap<Data.DomainUser, Model.DomainUser>(domainUser2);

        //    var membershipUser = Membership.GetUser(membershipUserName);
        //    domainUser.MembershipUser = Model.MembershipUser.ConvertToPleiades(membershipUser);
        //    return domainUser;
        //}


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
