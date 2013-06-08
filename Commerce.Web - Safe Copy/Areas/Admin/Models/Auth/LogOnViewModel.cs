using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Web.Areas.Admin.Models
{
    public class LogOnViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
