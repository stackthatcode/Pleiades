using Pleiades.Commerce.Domain.Interface;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Domain.Concrete
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
            var membershipUser = this.MembershipService.CreateUser(membershipUserRequest, out outStatus);
            if (outStatus != PleiadesMembershipCreateStatus.Success)
            {
                return null;
            }

            var identityUser = this.IdentityUserService.Create(identityUserRequest);

            var aggegrateUser = new AggregateUser
            {
                IdentityUser = identityUser,
                MembershipUserName = membershipUser.UserName,
            };

            this.AggregateUserRepository.Add(aggegrateUser);
            this.AggregateUserRepository.SaveChanges();

            return aggegrateUser;
        }

    }
}
