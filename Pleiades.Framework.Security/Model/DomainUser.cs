using System;

namespace Pleiades.Framework.Identity.Model
{
    /// <summary>
    /// Root Aggregate for Domain & Membership User objects
    /// </summary>
    public class DomainUser
    {
        // Domain specific data
        public int ID { get; set; }

        // Authorization properties
        public UserRole UserRole { get; set; }
        public AccountStatus? AccountStatus { get; set; }
        public AccountLevel? AccountLevel { get; set; }
        
        // Profile data
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Other
        public DateTime LastModified { get; set; }
        public DateTime CreationDate { get; set; }

        public DomainUser()
        {
            UserRole = UserRole.Anonymous;
        }
    }
}
