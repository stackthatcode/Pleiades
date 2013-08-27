using System;
using Pleiades.Application.Utility;

namespace Pleiades.Web.Security.Model
{
    /// <summary>
    /// Identity User object
    /// </summary>
    public class IdentityProfile
    {
        public IdentityProfile()
        {
            this.UserRole = UserRole.Anonymous;
            this.AccountLevel = AccountLevel.NotApplicable;
            this.AccountStatus = AccountStatus.NotApplicable;
        }

        // Domain specific data
        public int ID { get; set; }

        // Entity Framework stuff
        public string UserRoleValue { get; set; }
        public string AccountStatusValue { get; set; }
        public string AccountLevelValue { get; set; }


        // Authorization properties
        public UserRole UserRole 
        {
            get { return UserRoleValue.ParseToEnum<UserRole>(); }
            set { this.UserRoleValue = value.ToString(); }
        }

        public AccountStatus AccountStatus 
        {
            get { return AccountStatusValue.ParseToEnum<AccountStatus>(); }
            set { this.AccountStatusValue = value.ToString(); }
        }

        public AccountLevel AccountLevel
        {
            get { return AccountLevelValue.ParseToEnum<AccountLevel>(); }
            set { this.AccountLevelValue = value.ToString(); }
        }

        // Profile data
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}