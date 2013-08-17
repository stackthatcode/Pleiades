using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Pleiades.Data;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Providers;
using Pleiades.Web.Security.Utility;

namespace Pleiades.Web.Security.Concrete
{
    public class AggregateUserService : IAggregateUserService
    {
        public const int MaxSupremeUsers = 1;
        public const int MaxAdminUsers = 5;

        private Guid _tracer = Guid.NewGuid();
        public Guid Tracer { get { return _tracer; } }

        private static 
            ConcurrentDictionary<string, CacheEntry<AggregateUser>>
                _cache = new ConcurrentDictionary<string, CacheEntry<AggregateUser>>();

        public Func<string, AggregateUser> GetUserFromCache = (username) =>
            {
                if (!_cache.ContainsKey(username))
                {
                    return null;
                }
                CacheEntry<AggregateUser> entry;
                var success = _cache.TryGetValue(username, out entry);
                if (success == false)
                {
                    return null;
                }

                if (entry.LastRefreshed.AddSeconds(5) < DateTime.UtcNow)
                {
                    _cache.TryRemove(username, out entry);
                    return null;
                }

                return entry.Item;
            };

        public Action<AggregateUser> PutUserInCache = (user) =>
            {
                _cache.TryAdd(user.Membership.UserName, new CacheEntry<AggregateUser>(user));
            };


        public IAggregateUserRepository Repository { get; set; }
        public IMembershipService MembershipService { get; set; }
        public IFormsAuthenticationService FormsService { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }

        public AggregateUserService(
                IMembershipService membershipService, 
                IAggregateUserRepository aggregateUserRepository,
                IFormsAuthenticationService formsAuthenticationService,
                IUnitOfWork unitOfWork)
        {
            this.MembershipService = membershipService;
            this.Repository = aggregateUserRepository;
            this.FormsService = formsAuthenticationService;
            this.UnitOfWork = unitOfWork;
        }

        public bool Authenticate(string username, string password, bool persistenceCookie, List<UserRole> expectedRoles)
        {
            var membershipUser = this.MembershipService.ValidateUserByEmailAddr(username, password);

            if (membershipUser == null)
            {
                this.FormsService.ClearAuthenticationCookie();
                return false;
            }

            var aggregateuser = this.Repository.RetrieveByMembershipUserName(membershipUser.UserName);

            if (!expectedRoles.Contains(aggregateuser.IdentityProfile.UserRole))
            {
                this.FormsService.ClearAuthenticationCookie();
                return false;
            }

            // Success!
            this.FormsService.SetAuthCookieForUser(membershipUser.UserName, persistenceCookie);
            return true;
        }

        public AggregateUser LoadAuthentedUserIntoContext(HttpContextBase context)
        {
            var httpContextUser = context.AggregateUser();
            if (httpContextUser != null)
            {
                return httpContextUser;
            }

            var userName = context.MembershipUserName();
            if (userName == null)
            {
                var user = AggregateUser.AnonymousFactory();
                context.AggregateUser(user);
                return user;
            }

            var cachedUser = this.GetUserFromCache(userName);
            if (cachedUser != null)
            {
                Debug.WriteLine("Cache hit - user: " + userName);
                context.AggregateUser(cachedUser);
                return cachedUser;
            }

            Debug.WriteLine("Cached miss - user: " + userName);
            var currentUser = this.Repository.RetrieveByMembershipUserName(userName);

            if (currentUser == null)
            {
                this.FormsService.ClearAuthenticationCookie();
                var user = AggregateUser.AnonymousFactory();
                context.AggregateUser(user);
                return user;
            }

            context.AggregateUser(currentUser);
            this.PutUserInCache(currentUser);
            this.MembershipService.Touch(userName);
            return currentUser;
        } 

        public AggregateUser Create(CreateNewMembershipUserRequest membershipUserRequest, 
                CreateOrModifyIdentityRequest identityUserRequest, out PleiadesMembershipCreateStatus outStatus)
        {
            if (identityUserRequest.UserRole == UserRole.Anonymous)
            {
                throw new Exception("Can't create an Anonymous User");
            }

            if (identityUserRequest.UserRole == UserRole.Admin && 
                this.Repository.GetUserCountByRole(UserRole.Admin) >= MaxAdminUsers)
            {
                throw new Exception(String.Format("Maximum number of Admin Users is {0}", MaxAdminUsers));
            }

            if (identityUserRequest.UserRole == UserRole.Supreme &&
                this.Repository.GetUserCountByRole(UserRole.Supreme) >= MaxSupremeUsers)
            {
                throw new Exception(String.Format("Maximum number of Supreme Users is 1", MaxSupremeUsers));
            }
            
            // Create Membership User... does this being here?
            var membershipUser = this.MembershipService.CreateUser(membershipUserRequest, out outStatus);
            if (outStatus != PleiadesMembershipCreateStatus.Success)
            {
                return null;
            }

            // Get the Membership User
            var membershipUserThisContext = Repository.RetreiveMembershipUser(membershipUser.UserName);

            var aggegrateUser = new AggregateUser
            {
                Membership = membershipUser,
                IdentityProfile =  new Model.IdentityProfile
                {
                    AccountStatus = identityUserRequest.AccountStatus.Value,
                    UserRole = identityUserRequest.UserRole.Value,
                    AccountLevel = identityUserRequest.AccountLevel.Value,
                    FirstName = identityUserRequest.FirstName,
                    LastName = identityUserRequest.LastName,
                }
            };

            this.Repository.Insert(aggegrateUser);
            this.UnitOfWork.SaveChanges();
            return aggegrateUser;
        }

        public void UpdateIdentity(CreateOrModifyIdentityRequest identityUserRequest)
        {
            if (identityUserRequest.Email == null)
            {
                throw new Exception("Cannot set email address to null");
            }

            this.Repository.UpdateIdentity(identityUserRequest);
            var user = this.Repository.RetrieveById(identityUserRequest.Id);

            // NOW: possibly create another Service method UpdateEmailAndApproval..
            if (user.IdentityProfile.UserRole != UserRole.Supreme)
            {
                var membershipUserName = user.Membership.UserName;
                this.MembershipService.ChangeEmailAddress(membershipUserName, identityUserRequest.Email);
                this.MembershipService.SetUserApproval(membershipUserName, identityUserRequest.IsApproved);
            }

            this.UnitOfWork.SaveChanges();
        }

        public void ChangeUserPassword(int targetUserId, string oldPassword, string newPassword)
        {
            var user = this.Repository.RetrieveById(targetUserId);
            this.MembershipService.ChangePassword(user.Membership.UserName, oldPassword, newPassword);
            this.UnitOfWork.SaveChanges();
        }
    }
}