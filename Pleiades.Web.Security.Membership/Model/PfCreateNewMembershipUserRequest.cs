namespace Pleiades.Web.Security.Model
{
    public class PfCreateNewMembershipUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PasswordQuestion { get; set;  }
        public string PasswordAnswer { get; set;  }
        public bool IsApproved { get; set; }
    }
}