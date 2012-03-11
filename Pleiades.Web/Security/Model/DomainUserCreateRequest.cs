using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Web.Security.Model
{
    public class DomainUserCreateRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string PasswordQuestion { get; set;  }
        public string PasswordAnswer { get; set;  }
        public bool IsApproved { get; set; }

        public AccountStatus AccountStatus { get; set; }
        public UserRole UserRole { get; set; }
        public AccountLevel AccountLevel { get; set;  }
        public string FirstName { get; set; }
        public string LastName { get; set;}
    }
}
