using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using PagedList;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Data;
using Pleiades.Web.Security.Model;
using Pleiades.Utilities.General;
using AutoMapper;

namespace Pleiades.Web.Security.Concrete
{
    /// <summary>
    /// Core Domain User Service
    /// </summary>
    public class DomainUserService : IDomainUserService
    {
        public const int MaximumNumberOfAdmins = 3;

        // TODO: replace this with encrypted values stored in the Assembly manifest - or do the deploy thingy
        private const string defaultCode = "#3#3#3#3";
        private const string defaultEmail = "aleksjones@gmail.com";
        private const string defaultQuestion = "Dad's name";
        private const string defaultAnswer = "Donald";

        private const int MaxRootUsers = 1;
        private const int MaxAdminUsers = 5;


        /// <summary>
        /// Ensures the Root User Account Exists
        /// 
        /// TODO: separate this capability and create a standalone applet that handles this - make it part of 
        /// ... the deployment process
        /// </summary>
        public void Initialize()
        {
            // If Root User does not exist, then we have to create it
            var rootUsers = this.RetreiveAll(1, 999, new List<UserRole>() { UserRole.Root });
            if (rootUsers.Count > 0)
            {
                return;
            }

            // Create the Root User using defaults
            var username = GenerateUserName();
            MembershipCreateStatus status;

            this.Create(
                new DomainUserCreateRequest()
                {
                    Password = defaultCode,
                    Email = defaultEmail,
                    PasswordQuestion = defaultQuestion,
                    PasswordAnswer = defaultAnswer,
                    IsApproved = true,
                    AccountStatus = Model.AccountStatus.Active,
                    UserRole = UserRole.Root,
                    AccountLevel = AccountLevel.Gold,
                    FirstName = "Aleks",
                    LastName = "Jones"
                }, out status);

            if (status != MembershipCreateStatus.Success)
            {
                throw new Exception("Unable to create the default Root User");
            }
        }

        /// <summary>
        /// Generate a new Domain User - avoid creating quantity of Root & Admin Users beyond threshhold
        /// </summary>
        public Model.DomainUser Create(DomainUserCreateRequest newUserRequest, out MembershipCreateStatus createStatus)
        {
            // Don't exceed the maximum quantity of Admins
            var pleiadesDB = new PleiadesDB();

            if (newUserRequest.UserRole == UserRole.Admin)
            {
                var countAdmin = this.RetreiveAll(1, 999, new List<UserRole>() { UserRole.Admin }).Count;
                if (countAdmin >= MaxAdminUsers)
                {
                    throw new Exception(String.Format("Maximum number of Admin Users is {0}", MaxAdminUsers));
                }
            }

            if (newUserRequest.UserRole == UserRole.Root)
            {
                var countRoot = this.RetreiveAll(1, 999, new List<UserRole>() { UserRole.Root }).Count;
                if (countRoot >= MaxRootUsers)
                {
                    throw new Exception(String.Format("Maximum number of Root Users is 1", MaxRootUsers));
                }
            }

            // This will invoke the Membership Provider
            var generatedUserName = this.GenerateUserName();

            var membershipuser = Membership.CreateUser(
                    generatedUserName, newUserRequest.Password, newUserRequest.Email, newUserRequest.PasswordQuestion,
                    newUserRequest.PasswordAnswer, newUserRequest.IsApproved, out createStatus);

            // Failure due to Membership Provider
            if (createStatus != MembershipCreateStatus.Success)
            {
                return null;
            }

            // Get the Membership User
            var membershipUser = pleiadesDB.Users.First(x => x.Id == (Guid)membershipuser.ProviderUserKey);

            // Create the Domain User EF Entity and Save
            var domainUserEntity = new Data.DomainUser
            {
                User = membershipUser,
                AccountStatus = (int)newUserRequest.AccountStatus,
                UserRole = newUserRequest.UserRole.ToString(),
                AccountLevel = (int)newUserRequest.AccountLevel,
                FirstName = newUserRequest.FirstName,
                LastName = newUserRequest.LastName,
                CreationDate = DateTime.Now,
                LastModified = DateTime.Now,
            };
            pleiadesDB.DomainUsers.AddObject(domainUserEntity);
            pleiadesDB.SaveChanges();

            // Get the Domain User Aggregate we just added
            return RetrieveUserByDomainUserId(domainUserEntity.DomainUserId);
        }

        /// <summary>
        /// Creates a random 7-digit number for User Name (since we authenticate by email addy)
        /// </summary>
        private string GenerateUserName()
        {
            var username = "";
            var counter = 0;
            while (counter++ < 100)
            {
                var random = new Random();
                username = random.Next(9999999).ToString("D7");

                if (Membership.FindUsersByName(username).Count == 0)
                {
                    return username;
                }
            }

            throw new Exception("Unable to generate new user name after 100 tries");
        }

        /// <summary>
        /// Retrieve Domain User by ID
        /// </summary>
        public Model.DomainUser RetrieveUserByDomainUserId(int domainUserId)
        {
            var pleiadesDB = new PleiadesDB();
            var domainUserEntity = pleiadesDB.DomainUsers.First(x => x.DomainUserId == domainUserId);

            if (domainUserEntity == null)
                return null;

            return RetrieveHelper(domainUserEntity.User.Username, domainUserEntity);
        }

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
        /// Worker method gets Users by User Name
        /// </summary>
        private Model.DomainUser RetrieveHelper(string membershipUserName, Data.DomainUser domainUserEntity)
        {
            var domainUser = new Model.DomainUser()
            {
                DomainUserId = domainUserEntity.DomainUserId,
                UserRole = domainUserEntity.UserRole.StringToEnum<UserRole>(),
                AccountLevel = (Model.AccountLevel)domainUserEntity.AccountLevel,
                AccountStatus = (Model.AccountStatus)domainUserEntity.AccountStatus,
                FirstName = domainUserEntity.FirstName,
                LastName = domainUserEntity.LastName,
                LastModified = domainUserEntity.LastModified,
                CreationDate = domainUserEntity.CreationDate,
            };

            // Lil experiment
            // var domainUser2 = new Model.DomainUser();
            // domainUserEntity.AutoMap<Data.DomainUser, Model.DomainUser>(domainUser2);
            
            var membershipUser = Membership.GetUser(membershipUserName);
            domainUser.MembershipUser = Model.MembershipUser.ConvertToPleiades(membershipUser);
            return domainUser;
        }

        /// <summary>
        /// Retrieve All method with built-in Paging and User Role filtering
        /// </summary>
        public IPagedList<DomainUserCondensed> RetreiveAll(int pageNumber, int pageSize, List<UserRole> role)
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
        public IPagedList<DomainUserCondensed> RetreiveByLikeEmail(
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

        private IPagedList<DomainUserCondensed> RetrieveUserListItemOutput(
                IQueryable<DomainUserCondensed> records, int pageNumber, int pageSize)
        {
            var count = records.Count();
            var pagedresults = records.Page<DomainUserCondensed>(pageNumber, pageSize);
            return new StaticPagedList<DomainUserCondensed>(pagedresults, pageNumber - 1, pageSize, count);
        }

        /// <summary>
        /// Retrieve total head count
        /// </summary>
        public int RetrieveTotalUsers()
        {
            var pleiadesDB = new PleiadesDB();
            return pleiadesDB.DomainUsers.Count();
        }

        /// <summary>
        /// Update existing Domain User - will only modify Domain User entities, not Membership!
        /// </summary>
        public void Update(Model.DomainUser domainUser)
        {
            var pleiadesDB = new PleiadesDB();
            Data.DomainUser domainUserEntity 
                = pleiadesDB.DomainUsers.First(x => x.DomainUserId == domainUser.DomainUserId);

            domainUserEntity.UserRole = domainUser.UserRole.ToString();
            domainUserEntity.AccountStatus = (int)domainUser.AccountStatus;
            domainUserEntity.AccountLevel = (int)domainUser.AccountLevel;
            domainUserEntity.FirstName = domainUser.FirstName;
            domainUserEntity.LastName = domainUser.LastName;
            domainUserEntity.LastModified = DateTime.Now;
            pleiadesDB.SaveChanges();
        }

        /// <summary>
        /// Set the Last Modified footprint
        /// </summary>
        public void UpdateLastModified(Model.DomainUser domainUser)
        {
            var pleiadesDB = new PleiadesDB();
            Data.DomainUser domainUserEntity = pleiadesDB.DomainUsers.First(x => x.DomainUserId == domainUser.DomainUserId);
            domainUserEntity.LastModified = DateTime.Now;
            pleiadesDB.SaveChanges();
        }

        /// <summary>
        /// Delete Domain User
        /// </summary>
        /// <param name="domainUser"></param>
        public void Delete(Model.DomainUser domainUser)
        {
            var pleiadesDB = new PleiadesDB();
            var domainUserEntity = pleiadesDB.DomainUsers.First(x => x.DomainUserId == domainUser.DomainUserId);
            if (domainUserEntity.UserRole == UserRole.Root.ToString())
            {
                throw new Exception("Illegal to delete Root User from application layer");
            }

            var userName = domainUserEntity.User.Username;

            pleiadesDB.DomainUsers.DeleteObject(domainUserEntity);
            pleiadesDB.SaveChanges();

            Membership.DeleteUser(userName);
        }
    }
}