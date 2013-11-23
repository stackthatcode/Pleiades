using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Utility;

namespace Pleiades.Web.Security.Concrete
{
    //
    // TODO: add Logging for the Cache Miss/Hit stuff on DEBUG
    //
    public class AggregateUserService : IAggregateUserService
    {
        public const int MaxRootUsers = 2;
        public const int MaxAdminUsers = 5;

        public IPfMembershipService MembershipService { get; set; }
        public IReadOnlyAggregateUserRepository ReadOnlyRepository { get; set; }
        public IWritableAggregateUserRepository WritableRepository { get; set; }
        public IFormsAuthenticationService FormsService { get; set; }

        private static readonly ConcurrentDictionary<string, CacheEntry<AggregateUser>> _cache;

        public AggregateUserService(
                IPfMembershipService membershipService, 
                IReadOnlyAggregateUserRepository aggregateUserRepository,
                IWritableAggregateUserRepository writableAggregateUserRepository, 
                IFormsAuthenticationService formsAuthenticationService)
        {
            this.MembershipService = membershipService;
            this.ReadOnlyRepository = aggregateUserRepository;
            this.WritableRepository = writableAggregateUserRepository;
            this.FormsService = formsAuthenticationService;
        }

        static AggregateUserService()
        {
            _cache = new ConcurrentDictionary<string, CacheEntry<AggregateUser>>();
        }

        public Action<AggregateUser> PutUserInCache = 
            (user) => _cache.TryAdd(user.Membership.UserName, new CacheEntry<AggregateUser>(user));
        
        public Func<string, AggregateUser> GetUserFromCache = 
            (username) =>
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

        public AggregateUser Authenticate(string username, string password, bool persistenceCookie, List<UserRole> expectedRoles)
        {
            var membershipUser = this.MembershipService.ValidateUserByEmailAddr(username, password);

            if (membershipUser == null)
            {
                this.FormsService.ClearAuthenticationCookie();
                return null;
            }

            var aggregateuser = this.ReadOnlyRepository.RetrieveByMembershipUserName(membershipUser.UserName);
            if (!expectedRoles.Contains(aggregateuser.IdentityProfile.UserRole))
            {
                this.FormsService.ClearAuthenticationCookie();
                return null;
            }

            // Success!
            this.FormsService.SetAuthCookieForUser(membershipUser.UserName, persistenceCookie);
            return aggregateuser;
        }

        public AggregateUser LoadAuthentedUserIntoContext(HttpContextBase context)
        {
            var httpContextUser = context.RetreiveAggregateUserFromContext();
            if (httpContextUser != null)
            {
                return httpContextUser;
            }

            var userName = context.ExtractUserNameFromContext();
            if (userName == null)
            {
                var user = AggregateUser.AnonymousFactory();
                context.StoreAggregateUserInContext(user);
                return user;
            }

            // NOTE: doesn't this stuff belong in the ReadOnlyRepository...?
            var cachedUser = this.GetUserFromCache(userName);
            if (cachedUser != null)
            {
                // TODO: logging
                Debug.WriteLine("Cache hit - user: " + userName);
                context.StoreAggregateUserInContext(cachedUser);
                return cachedUser;
            }

            // TODO: logging
            Debug.WriteLine("Cached miss - user: " + userName);
            var currentUser = this.ReadOnlyRepository.RetrieveByMembershipUserName(userName);

            if (currentUser == null)
            {
                this.FormsService.ClearAuthenticationCookie();
                var user = AggregateUser.AnonymousFactory();
                context.StoreAggregateUserInContext(user);
                return user;
            }

            context.StoreAggregateUserInContext(currentUser);
            this.PutUserInCache(currentUser);
            this.MembershipService.Touch(userName);
            return currentUser;
        } 

        public AggregateUser Create(PfCreateNewMembershipUserRequest membershipUserRequest, 
                IdentityProfileChange identityUserChange, out string message)
        {
            if (identityUserChange.UserRole == UserRole.Anonymous)
            {
                message = "Can't create an Anonymous User";
                return null;
            }

            if (identityUserChange.UserRole == UserRole.Admin && ReadOnlyRepository.GetUserCountByRole(UserRole.Admin) >= MaxAdminUsers)
            {
                message = String.Format("Maximum number of Admin Users is {0}", MaxAdminUsers);
                return null;
            }

            if (identityUserChange.UserRole == UserRole.Root && ReadOnlyRepository.GetUserCountByRole(UserRole.Root) >= MaxRootUsers)
            {
                message = String.Format("Maximum number of Root Users is {0}", MaxRootUsers);
                return null;
            }
            
            // Create Membership User... does this being here?
            PfMembershipCreateStatus outStatus;
            var membershipUser = this.MembershipService.CreateUser(membershipUserRequest, out outStatus);
            if (outStatus != PfMembershipCreateStatus.Success)
            {
                message = "Membership Failure: " + outStatus.ToString();
                return null;
            }

            // Create Aggregate User object record
            var aggegrateUser = new AggregateUser
            {
                Membership = membershipUser,
                IdentityProfile =  new IdentityProfile
                {
                    AccountStatus = identityUserChange.AccountStatus.Value,
                    UserRole = identityUserChange.UserRole.Value,
                    AccountLevel = identityUserChange.AccountLevel.Value,
                    FirstName = identityUserChange.FirstName,
                    LastName = identityUserChange.LastName,
                }
            };

            this.WritableRepository.Add(aggegrateUser);
            message = "Success";
            return aggegrateUser;
        }

        public void UpdateIdentity(int id, IdentityProfileChange identityChange)
        {
            var user = this.WritableRepository.RetrieveById(id);

            if (identityChange.AccountStatus != null)
                user.IdentityProfile.AccountStatus = identityChange.AccountStatus.Value;
            if (identityChange.UserRole != null && identityChange.UserRole != UserRole.Root)
                user.IdentityProfile.UserRole = identityChange.UserRole.Value;
            if (identityChange.AccountLevel != null)
                user.IdentityProfile.AccountLevel = identityChange.AccountLevel.Value;
            if (identityChange.FirstName != null)
                user.IdentityProfile.FirstName = identityChange.FirstName;
            if (identityChange.LastName != null)
                user.IdentityProfile.LastName = identityChange.LastName;
        }


        public void Delete(int id)
        {
            this.WritableRepository.Delete(id);
        }
    }
}
