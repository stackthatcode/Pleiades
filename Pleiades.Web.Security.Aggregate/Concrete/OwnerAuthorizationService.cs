using System;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Concrete
{
    public class OwnerAuthorizationService : IOwnerAuthorizationService
    {
        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IHttpContextUserService HttpContextUserService { get; set; }

        public OwnerAuthorizationService(
                IAggregateUserRepository aggregateUserRepository, IHttpContextUserService httpContextUserService)
        {
            this.AggregateUserRepository = aggregateUserRepository;
            this.HttpContextUserService = httpContextUserService;
        }

        public SecurityCode Authorize(int ownerUserId)
        {
            var requestingUser = this.HttpContextUserService.Get();
            var ownerUser = this.AggregateUserRepository.RetrieveById(ownerUserId);

            return this.Authorize(requestingUser, ownerUser);
        }

        public SecurityCode Authorize(AggregateUser requestingUser, AggregateUser ownerUser)
        {
            if (requestingUser.IdentityProfile.UserRole == UserRole.Admin &&
                ownerUser.IdentityProfile.UserRole == UserRole.Supreme)
            {
                return SecurityCode.AccessDenied;
            }

            if (requestingUser.IdentityProfile.UserRole.IsAdministratorOrSupreme())
            {
                return SecurityCode.Allowed;
            }

            if (ownerUser == null)
            {
                return SecurityCode.Allowed;
            }

            if (requestingUser.IdentityProfile.ID == ownerUser.IdentityProfile.ID)
            {
                return SecurityCode.Allowed;
            }

            return SecurityCode.AccessDenied;
        }
    }
}