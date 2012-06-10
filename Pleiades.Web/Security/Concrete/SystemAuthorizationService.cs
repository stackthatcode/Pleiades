using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Model
{
    public class SystemAuthService : ISystemAuthorizationService
    {
        /// <summary>
        /// 
        /// </summary>
        public SecurityResponseCode Authorize(DomainUser user, SecurityRequirementsContext context)
        {
            var roleAuthorizationResponse = RoleAuthorization(user, context);

            if (roleAuthorizationResponse != SecurityResponseCode.Allowed)
                return roleAuthorizationResponse;

            var accountstatusauth = AccountStatusAuthorization(user, context);
            var accountlevelauth = AccountLevelAuthorization(user, context);
            if (accountstatusauth != SecurityResponseCode.Allowed)
                return accountstatusauth;

            if (accountlevelauth != SecurityResponseCode.Allowed)
                return accountlevelauth;

            return SecurityResponseCode.Allowed;
        }

        private SecurityResponseCode RoleAuthorization(DomainUser user, SecurityRequirementsContext context)
        {
            // Public areas are accessible by everybody
            if (context.AuthorizationZone == AuthorizationZone.Public)
            {
                return SecurityResponseCode.Allowed;
            }

            // Admins have no barriers
            if (user.UserRole.IsAdministrator())
            {
                return SecurityResponseCode.Allowed;
            }

            // Reject anyone that's in an Admin area that's not an Admin
            if (context.AuthorizationZone == AuthorizationZone.Administrative
                && user.UserRole.IsNotAdministrator())
            {
                return SecurityResponseCode.AccessDenied;
            }

            // Reject anyone that's not Trusted in a Trusted area; solicit for Logon
            if (context.AuthorizationZone == AuthorizationZone.Restricted
                && user.UserRole != UserRole.Trusted
                && user.UserRole.IsNotAdministrator())
            {
                return SecurityResponseCode.AccessDeniedSolicitLogon;
            }

            // Role Authorization - Pass
            return SecurityResponseCode.Allowed;
        }

        private SecurityResponseCode AccountStatusAuthorization(DomainUser user, SecurityRequirementsContext context)
        {
            if (user.UserRole.IsAdministrator())
                return SecurityResponseCode.Allowed;

            if (context.AuthorizationZone != AuthorizationZone.Restricted)
                return SecurityResponseCode.Allowed;

            if (user.AccountStatus == null)
                return SecurityResponseCode.AccessDeniedSolicitLogon;

            if (user.AccountStatus == AccountStatus.Disabled)
                return SecurityResponseCode.AccessDeniedSolicitLogon;

            if (user.AccountStatus == AccountStatus.PaymentRequired && context.PaymentArea != true)
                return SecurityResponseCode.AccountDisabledNonPayment;

            return SecurityResponseCode.Allowed;
        }

        private SecurityResponseCode AccountLevelAuthorization(DomainUser user, SecurityRequirementsContext context)
        {
            if (user.UserRole.IsAdministrator())
                return SecurityResponseCode.Allowed;

            // Only applies to Restricted Areas
            if (context.AuthorizationZone != AuthorizationZone.Restricted)
                return SecurityResponseCode.Allowed;

            if (context.AccountLevelRestriction == AccountLevel.Standard)
                return SecurityResponseCode.Allowed;

            if (context.AccountLevelRestriction == AccountLevel.Gold && user.AccountLevel != AccountLevel.Gold)
                return SecurityResponseCode.AccessDeniedToAccountLevel;

            return SecurityResponseCode.Allowed;
        }
    }
}
