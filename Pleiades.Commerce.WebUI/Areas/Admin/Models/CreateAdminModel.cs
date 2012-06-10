using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Framework.Helpers;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Models
{
    public class CreateAdminModel
    {
        [Required]
        [DisplayName("First Name")]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [MaxLength(25)]
        public string LastName { get; set; }

        // Membership Data read-write
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        [PasswordPropertyText]
        [MaxLength(25)]
        public string Password { get; set; }

        [Required]
        [PasswordPropertyText]
        [DisplayName("Password Verify")]
        [MaxLength(25)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordVerify { get; set; }

        // Membership Data read-only
        [Required]
        [DisplayName("Is Approved")]
        public bool IsApproved { get; set; }
    }
}