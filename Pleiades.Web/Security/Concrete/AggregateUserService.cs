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
            var membershipUser = this.MembershipService.CreateUser(membershipUserRequest, out outStatus);
            if (outStatus != PleiadesMembershipCreateStatus.Success)
            {
                return null;
            }

            var identityUser = this.IdentityUserService.Create(identityUserRequest);

            var aggegrateUser = new AggregateUser
            {
                MembershipUserName = membershipUser.UserName,
            };

            this.AggregateUserRepository.Add(aggegrateUser);
            this.AggregateUserRepository.SaveChanges();

            aggegrateUser.IdentityUser = identityUser;
            this.AggregateUserRepository.SaveChanges();

            return aggegrateUser;
        }
    }
}
