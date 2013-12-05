using System.ComponentModel.DataAnnotations;

namespace Commerce.Web.Models.Manager
{
    public class ResetPasswordModel
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
