using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Commerce.Web.Models.Manager
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "System Error - please try again")]
        public int AggregateUserId { get; set; }

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [PasswordPropertyText]
        [DisplayName("Password Verify")]
        [MaxLength(25)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordVerify { get; set; }
    }
}
