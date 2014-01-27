namespace Commerce.Web.Models.Auth
{
    public class PasswordChangeModel
    {
        public string ExternalGuid { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
