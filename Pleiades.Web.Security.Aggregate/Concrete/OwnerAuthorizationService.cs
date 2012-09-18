using System;
using Pleiades.Security;
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

        public SecurityResponseCode Authorize(int ownerUserId)
        {
            var requestingUser = this.HttpContextUserService.GetCurrentUserFromHttpContext();
            var ownerUser = this.AggregateUserRepository.RetrieveById(ownerUserId);

            return this.Authorize(requestingUser, ownerUser);
        }

        public SecurityResponseCode Authorize(AggregateUser requestingUser, AggregateUser ownerUser)
        {
            if (requestingUser.IdentityProfile.UserRole == UserRole.Admin &&
                ownerUser.IdentityProfile.UserRole == UserRole.Supreme)
            {
                return SecurityResponseCode.AccessDenied;
            }

            if (requestingUser.IdentityProfile.UserRole.IsAdministratorOrSupreme())
            {
                return SecurityResponseCode.Allowed;
            }

            if (ownerUser == null)
            {
                return SecurityResponseCode.Allowed;
            }

            if (requestingUser.IdentityProfile.ID == ownerUser.IdentityProfile.ID)
            {
                return SecurityResponseCode.Allowed;
            }

            return SecurityResponseCode.AccessDenied;
        }
    }
}