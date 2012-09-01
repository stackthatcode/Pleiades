using System.Linq;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Concrete
{
    public class AggregateUserService : IAggregateUserService
    {
        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IMembershipService MembershipService { get; set; }

        public AggregateUserService(
                IMembershipService membershipService, 
                IAggregateUserRepository aggregateUserRepository)
        {
            this.MembershipService = membershipService;
            this.AggregateUserRepository = aggregateUserRepository;
        }

        public AggregateUser Create(
                CreateNewMembershipUserRequest membershipUserRequest, 
                CreateOrModifyIdentityUserRequest identityUserRequest,
                out PleiadesMembershipCreateStatus outStatus)
        {
            // Create Membership User... does this belong here?
            var membershipUser = this.MembershipService.CreateUser(membershipUserRequest, out outStatus);
            if (outStatus != PleiadesMembershipCreateStatus.Success)
            {
                return null;
            }

            // Get the Membership User
            var membershipUserThisContext = AggregateUserRepository.RetreiveMembershipUser(membershipUser.UserName);

            var identityUser = this.IdentityUserService.Create(identityUserRequest);
            var aggegrateUser = new AggregateUser
            {
                Membership = membershipUserThisContext,
                Identity = identityUser,
            };
            this.AggregateUserRepository.Add(aggegrateUser);
            this.AggregateUserRepository.SaveChanges();

            return aggegrateUser;
        }
    }
}