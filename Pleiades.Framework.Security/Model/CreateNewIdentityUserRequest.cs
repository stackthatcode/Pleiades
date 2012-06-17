using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Framework.Identity.Model
{
    public class CreateNewIdentityUserRequest
    {
        public AccountStatus AccountStatus { get; set; }
        public IdentityUserRole UserRole { get; set; }
        public AccountLevel AccountLevel { get; set;  }
        public string FirstName { get; set; }
        public string LastName { get; set;}
    }
}
