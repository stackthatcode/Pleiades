﻿using System;
using System.Web.Mvc;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Commerce.WebUI.Areas.Admin.Controllers;
using Pleiades.Commerce.WebUI.Areas.Public.Controllers;

namespace Pleiades.Commerce.WebUI.Plumbing.Security
{
    public class CommerceAuthorizationRule : IAuthorizationRule 
    {
        public bool MustAuthorize(AuthorizationContext filterContext)
        {
            if (filterContext.Controller is ProductsController)
                return false;

            if (filterContext.Controller is LoginController)
                return false;

            return true;
        }
    }
}