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
    public class UserViewModel
    {
        // Domain specific data
        [ReadOnly(true)]
        [HiddenInput (DisplayValue = false)]
        public int DomainUserId { get; set; }

        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }

        public UserRole UserRole { get; set; }
        public AccountStatus? AccountStatus { get; set; }
        public AccountLevel? AccountLevel { get; set; }

        [Required]
        [DisplayName("First Name")]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [MaxLength(25)]
        public string LastName { get; set; }

        [ReadOnly(true)]
        [DisplayName("Date Created")]
        public DateTime CreationDate { get; set; }

        [ReadOnly(true)]
        [DisplayName("Last Modified")]
        public DateTime LastModified { get; set; }

        // Membership Data read-write
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        // Membership Data read-only
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }

        [ReadOnly(true)]
        public bool IsOnline { get; set; }


        public static UserViewModel Make(DomainUser user)
        {
            var userModel = user.AutoMap<DomainUser, UserViewModel>(new UserViewModel());
            user.MembershipUser.AutoMap<MembershipUser, UserViewModel>(userModel);
            return userModel;
        }

        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

    }
}