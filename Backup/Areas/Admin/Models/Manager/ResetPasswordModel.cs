using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.WebUI.Areas.Admin.Models
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
