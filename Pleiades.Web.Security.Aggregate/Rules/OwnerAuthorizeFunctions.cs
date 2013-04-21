using System;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Rules
{
    public class OwnerAuthorizeFunctions
    {
        public static SecurityCode Authorize(AggregateUser requestingUser, AggregateUser ownerUser)
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