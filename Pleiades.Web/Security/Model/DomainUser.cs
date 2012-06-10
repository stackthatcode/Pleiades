using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Pleiades.Framework.Web.Security.Model
{
    /// <summary>
    /// Root Aggregate for Domain & Membership User objects
    /// </summary>
    public class DomainUser
    {
        // Wrapper object for Membership Provider User
        public MembershipUser MembershipUser { get; set; }

        // Domain specific data
        public int DomainUserId { get; set; }
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
