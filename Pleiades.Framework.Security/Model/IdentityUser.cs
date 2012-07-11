﻿using System;

namespace Pleiades.Framework.Identity.Model
{
    /// <summary>
    /// Identity User object
    /// </summary>
    public class IdentityUser
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

        public IdentityUser()
        {
            UserRole = UserRole.Anonymous;
        }
    }
}