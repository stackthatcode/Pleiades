using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Framework.Web.Security.Model;


namespace Pleiades.Commerce.WebUI.Plumbing.Model
{
    public class DomainUserBinder : IModelBinder
    {
        public IDomainUserService DomainUserService { get; set; }
        public IHttpContextUserService HttpContextUserService { get; set; }

        public DomainUserBinder()
        {
            this.HttpContextUserService = new HttpContextUserService();
            this.DomainUserService = new DomainUserService();
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                throw new Exception("Cannot update instances");

            var user = HttpContextUserService.RetrieveUserFromHttpContext(controllerContext.HttpContext);
            return user;
        }
    }
}