﻿using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    public class AuthContextGenerator
    {
        public static SystemAuthorizationContext PublicArea(IdentityProfile user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Public,
                ThisUser = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }

        public static SystemAuthorizationContext RestrictedStandardNonPaymentArea(IdentityProfile user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = false,
                ThisUser = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }

        public static SystemAuthorizationContext RestrictedGoldNonPaymentArea(IdentityProfile user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Gold,
                IsPaymentArea = false,
                ThisUser = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }

        public static SystemAuthorizationContext RestrictedPaymentArea(IdentityProfile user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = true,
                ThisUser = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }

        public static SystemAuthorizationContext AdministrativeArea(IdentityProfile user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Administrative,
                ThisUser = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }
    }
}