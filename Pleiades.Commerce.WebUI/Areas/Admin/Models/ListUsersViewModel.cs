using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Models
{
    public class ListUsersViewModel 
    {
        public IPagedList<DomainUserCondensed> Users { get; set; }
    }
}
