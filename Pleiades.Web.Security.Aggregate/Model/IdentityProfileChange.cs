﻿namespace Pleiades.Web.Security.Model
{
    public class IdentityProfileChange
    {
        public AccountStatus? AccountStatus { get; set; }
        public UserRole? UserRole { get; set; }
        public AccountLevel? AccountLevel { get; set;  }
        public string FirstName { get; set; }
        public string LastName { get; set;}
    }
}