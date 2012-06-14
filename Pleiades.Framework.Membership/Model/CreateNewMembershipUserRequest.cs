using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Framework.MembershipProvider.Model
{
    public class CreateNewMembershipUserRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string PasswordQuestion { get; set;  }
        public string PasswordAnswer { get; set;  }
        public bool IsApproved { get; set; }
    }
}
