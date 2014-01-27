using System;

namespace Commerce.Application.Security
{
    public class PasswordResetLink
    {
        public int Id { get; set; }
        public Guid ExternalGuid { get; set; }
        public string MembershipUserName { get; set; }   
        public DateTime DateCreated { get; set; }
        public DateTime ExpirationDate { get; set; }

        public bool Expired
        {
            get
            {
                return ExpirationDate < DateTime.Now;
            }
        }
    }
}
