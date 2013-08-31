using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Rules
{
    public class OwnerAuthorizeRules
    {
        public static SecurityCode Authorize(AggregateUser requestingUser, AggregateUser ownerUser)
        {
            if (requestingUser.IdentityProfile.UserRole == UserRole.Admin &&
                ownerUser.IdentityProfile.UserRole == UserRole.Root)
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