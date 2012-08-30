using System.Linq;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Concrete
{
    public class AggregateUserService : IAggregateUserService
    {
        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IMembershipService MembershipService { get; set; }
        public IIdentityUserService IdentityUserService { get; set; }

        public AggregateUserService(
                IMembershipService membershipService, 
                IIdentityUserService identityUserService, 
                IAggregateUserRepository aggregateUserRepository)
        {
            this.MembershipService = membershipService;
            this.IdentityUserService = identityUserService;
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
                MembershipUser = membershipUserThisContext,
                IdentityUser = identityUser,
            };
            this.AggregateUserRepository.Add(aggegrateUser);
            this.AggregateUserRepository.SaveChanges();

            return aggegrateUser;
        }
    }
}