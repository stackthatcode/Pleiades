using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Rules
{
    public static class FluentSecurityRules
    {
        public static SecurityContext AccountLevelCheck(this SecurityContext context)
        {
            if (context.Pass == false)
            {
                return context;
            }

            if (context.User.IdentityProfile.UserRole.IsAdministratorOrSupreme())
            {
                return context;
            }

            // Only applies to Restricted Areas
            if (context.AuthorizationZone != AuthorizationZone.Restricted)
            {
                return context;
            }

            if (context.AccountLevelRestriction == AccountLevel.Standard)
            {
                return context;
            }

            if (context.AccountLevelRestriction == AccountLevel.Gold &&
                context.User.IdentityProfile.AccountLevel != AccountLevel.Gold)
            {
                return context.Fail(SecurityCode.AccessDeniedToAccountLevel);
            }

            return context;
        }

        public static SecurityContext AccountStatusCheck(this SecurityContext context)
        {
            if (context.Pass == false)
            {
                return context;
            }

            if (context.User.IdentityProfile.UserRole.IsAdministratorOrSupreme())
            {
                return context;
            }

            if (context.AuthorizationZone == AuthorizationZone.Public)
            {
                return context;
            }

            if (context.User.IdentityProfile.AccountStatus == null)
            {
                return context.Fail(SecurityCode.AccessDeniedSolicitLogon);
            }

            if (context.User.IdentityProfile.AccountStatus == AccountStatus.Disabled)
            {
                return context.Fail(SecurityCode.AccessDeniedSolicitLogon);
            }

            if (context.User.IdentityProfile.AccountStatus == AccountStatus.PaymentRequired &&
                context.IsPaymentArea != true)
            {
                return context.Fail(SecurityCode.AccountDisabledNonPayment);
            }

            // Keep stepping!
            return context;
        }

        public static SecurityContext UserRoleCheck(this SecurityContext context)
        {
            if (context.Pass == false)
            {
                return context;
            }

            // Public areas are accessible by everybody
            if (context.AuthorizationZone == AuthorizationZone.Public)
            {
                return context;
            }

            // Admins have no barriers
            if (context.User.IdentityProfile.UserRole.IsAdministratorOrSupreme())
            {
                return context;
            }

            // Reject anyone that's in an Admin area that's not an Admin
            if (context.AuthorizationZone == AuthorizationZone.Administrative && 
                context.User.IdentityProfile.UserRole.IsNotAdministratorOrSupreme())
            {
                return context.Fail(SecurityCode.AccessDeniedSolicitLogon);
            }

            // Reject anyone that's not Trusted in a Trusted area; solicit for Logon
            if (context.AuthorizationZone == AuthorizationZone.Restricted
                && context.User.IdentityProfile.UserRole != UserRole.Trusted
                && context.User.IdentityProfile.UserRole.IsNotAdministratorOrSupreme())
            {
                return context.Fail(SecurityCode.AccessDeniedSolicitLogon);
            }

            // No denials, keep stepping!
            return context;
        }
    }
}
