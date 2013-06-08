using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Web.Areas.Admin.Models
{
    public class UserViewModel
    {
        [ReadOnly(true)]
        [HiddenInput (DisplayValue = false)]
        public int AggregateUserId { get; set; }

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

        // Membership Data read-only -- why readonly...?
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; protected set; }

        [ReadOnly(true)]
        public bool IsOnline { get; set; }

        public UserViewModel()
        {
        }

        public UserViewModel(AggregateUser user)
        {
            AggregateUserId = user.ID;
            UserRole = user.IdentityProfile.UserRole;
            AccountLevel = user.IdentityProfile.AccountLevel;
            AccountStatus = user.IdentityProfile.AccountStatus;
            CreationDate = user.Membership.CreationDate;
            Email = user.Membership.Email;
            FirstName = user.IdentityProfile.FirstName;
            LastName = user.IdentityProfile.LastName;
            IsApproved = user.Membership.IsApproved;
            IsLockedOut = user.Membership.IsLockedOut ?? false;
            IsOnline = user.Membership.IsOnline;
            LastModified = user.Membership.LastModified;
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}