using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Providers;

namespace Pleiades.Web.Security.Concrete
{
    public class AggregateUserService : IAggregateUserService
    {
        public const int MaxSupremeUsers = 1;
        public const int MaxAdminUsers = 5;

        public IAggregateUserRepository Repository { get; set; }
        public IMembershipService MembershipService { get; set; }
        public IOwnerAuthorizationService OwnerAuthorizationService { get; set; }
        public IFormsAuthenticationService FormsService { get; set; }
        public IHttpContextUserService HttpContextUserService { get; set; }

        public AggregateUserService(
                IMembershipService membershipService, 
                IAggregateUserRepository aggregateUserRepository,
                IOwnerAuthorizationService ownerAuthorizationService,
                IFormsAuthenticationService formsAuthenticationService,
                IHttpContextUserService httpContextUserService)
        {
            this.MembershipService = membershipService;
            this.Repository = aggregateUserRepository;
            this.OwnerAuthorizationService = ownerAuthorizationService;
            this.FormsService = formsAuthenticationService;
            this.HttpContextUserService = httpContextUserService;
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

        public AggregateUser GetAuthenticatedUser(HttpContextBase context)
        {
            var httpContextUser = this.HttpContextUserService.Get();
            if (httpContextUser != null)
            {
                return httpContextUser;
            }

            var userName = context.MembershipUserName();
            if (userName == null)
            {
                var user = AggregateUser.AnonymousFactory();
                this.HttpContextUserService.Put(user);
                return user;
            }

            var currentUser = this.Repository.RetrieveByMembershipUserName(userName);
            if (currentUser == null)
            {
                var user = AggregateUser.AnonymousFactory();
                this.FormsService.ClearAuthenticationCookie();
                this.HttpContextUserService.Put(user);
                return user;
            }

            this.HttpContextUserService.Put(currentUser);
            this.MembershipService.Touch(userName);
            return currentUser;
        } 

        public AggregateUser Create(
                CreateNewMembershipUserRequest membershipUserRequest, 
                CreateOrModifyIdentityRequest identityUserRequest,
                out PleiadesMembershipCreateStatus outStatus)
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
            
            // Create Membership User... does this belong here?
            var membershipUser = this.MembershipService.CreateUser(membershipUserRequest, out outStatus);
            if (outStatus != PleiadesMembershipCreateStatus.Success)
            {
                return null;
            }

            // Get the Membership User
            var membershipUserThisContext = Repository.RetreiveMembershipUser(membershipUser.UserName);
            var aggegrateUser = new AggregateUser
            {
                Membership = membershipUserThisContext,
                IdentityProfile =  new Model.IdentityProfile
                {
                    AccountStatus = identityUserRequest.AccountStatus.Value,
                    UserRole = identityUserRequest.UserRole.Value,
                    AccountLevel = identityUserRequest.AccountLevel.Value,
                    FirstName = identityUserRequest.FirstName,
                    LastName = identityUserRequest.LastName,
                }
            };

            this.Repository.Add(aggegrateUser);
            this.Repository.SaveChanges();
            return aggegrateUser;
        }

        public void UpdateIdentity(int aggregateUserId, CreateOrModifyIdentityRequest identityUserRequest)
        {
            var securityCode = this.OwnerAuthorizationService.Authorize(aggregateUserId);
            if (securityCode != SecurityCode.Allowed)
            {
                throw new Exception("Current User does not have Authorization to do that: " + securityCode);
            }

            this.Repository.UpdateIdentity(
                        aggregateUserId,
                        new CreateOrModifyIdentityRequest
                        {
                            FirstName = identityUserRequest.FirstName,
                            LastName = identityUserRequest.LastName,
                        });
        }

        public void UpdateEmail(int aggregateUserId, string email)
        {
            var securityCode = this.OwnerAuthorizationService.Authorize(aggregateUserId);
            if (securityCode != SecurityCode.Allowed)
            {
                throw new Exception("Current User does not have Authorization to do that: " + securityCode);
            }

            if (email == null)
            {
                throw new Exception("Cannot set email address to null");
            }

            var userData = this.Repository.RetrieveById(aggregateUserId);
            var membershipUserName = userData.Membership.UserName;

            this.MembershipService.ChangeEmailAddress(membershipUserName, email);
        }

        public void UpdateApproval(int aggregateUserId, bool approval)
        {
            var securityCode = this.OwnerAuthorizationService.Authorize(aggregateUserId);
            if (securityCode != SecurityCode.Allowed)
            {
                throw new Exception("Current User does not have Authorization to do that: " + securityCode);
            }
            
            var userData = this.Repository.RetrieveById(aggregateUserId);
            var membershipUserName = userData.Membership.UserName;

            if (userData.IdentityProfile.UserRole != UserRole.Supreme)
            {
                MembershipService.SetUserApproval(membershipUserName, approval);
            }
        }

        public void ChangeUserPassword(int aggregateUserId, string oldPassword, string newPassword)
        {
            var user = this.Repository.RetrieveById(aggregateUserId);
            this.MembershipService.ChangePassword(user.Membership.UserName, oldPassword, newPassword);
        }
    }
}