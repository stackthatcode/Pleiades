using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Model;


namespace Pleiades.Commerce.WebUI.Plumbing.Model
{
    public class DomainUserBinder : IModelBinder
    {
        public IDomainUserService userService { get; set; }
        public IHttpContextUserService authService { get; set; }

        public DomainUserBinder()
        {
            authService = new HttpContextUserService();
            userService = new DomainUserService();
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                throw new Exception("Cannot update instances");

            var username = authService.GetUserNameFromContext(controllerContext.HttpContext);
            var user = userService.RetrieveUserByMembershipUserName(username);
            return user;
        }
    }
}