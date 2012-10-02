using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Commerce.WebUI.Areas.Admin.Models
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
