﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Model;

namespace Commerce.WebUI.Areas.Admin.Models
{
    public class ListUsersViewModel 
    {
        public IPagedList<DomainUserCondensed> Users { get; set; }
    }
}