using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Pleiades.Web.Security.Model;

namespace Commerce.Web.Models.Manager
{
    public class EditUserModel
    {
        [ReadOnly(true)]
        [HiddenInput (DisplayValue = false)]
        public int AggregateUserId { get; set; }

        public UserRole UserRole { get; set; }

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

        public bool IsApproved { get; set; }


        // For testing
        public EditUserModel()
        {
        }

        public EditUserModel(AggregateUser user)
        {
            AggregateUserId = user.ID;
            UserRole = user.IdentityProfile.UserRole;
            Email = user.Membership.Email;
            FirstName = user.IdentityProfile.FirstName;
            LastName = user.IdentityProfile.LastName;
            IsApproved = user.Membership.IsApproved;
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