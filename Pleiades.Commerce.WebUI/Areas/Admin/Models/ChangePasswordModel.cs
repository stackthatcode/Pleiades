using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.WebUI.Areas.Admin.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Please enter a valid email address")]
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
