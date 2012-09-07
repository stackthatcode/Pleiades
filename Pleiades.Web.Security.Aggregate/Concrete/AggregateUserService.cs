using System;
using System.Linq;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Concrete
{
    public class AggregateUserService : IAggregateUserService
    {
        public const int MaxSupremeUsers = 1;
        public const int MaxAdminUsers = 5;

        public IAggregateUserRepository Repository { get; set; }
        public IMembershipService MembershipService { get; set; }

        public AggregateUserService(
                IMembershipService membershipService, 
                IAggregateUserRepository aggregateUserRepository)
        {
            this.MembershipService = membershipService;
            this.Repository = aggregateUserRepository;
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
                    AccountStatus = identityUserRequest.AccountStatus,
                    UserRole = identityUserRequest.UserRole,
                    AccountLevel = identityUserRequest.AccountLevel,
                    FirstName = identityUserRequest.FirstName,
                    LastName = identityUserRequest.LastName,
                }
            };

            this.Repository.Add(aggegrateUser);
            this.Repository.SaveChanges();
            return aggegrateUser;
        }
    }
}