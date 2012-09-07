using System.Collections.Generic;
using Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Web.Security.Model;

namespace Commerce.WebUI.Areas.Admin.Models
{
    public class ListUsersViewModel 
    {
        public List<UserViewModel> Users { get; set; }
    }
}