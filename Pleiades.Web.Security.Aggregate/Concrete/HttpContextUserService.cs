﻿using System.Web;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Concrete
{
    public class HttpContextUserService : IHttpContextUserService
    {
        public AggregateUser Get()
        {
            return HttpContext.Current.Items["AggregateUserCache"] as AggregateUser;
        }

        public void Put(AggregateUser user)
        {
            HttpContext.Current.Items["AggregateUserCache"] = user;
        }
    }
}