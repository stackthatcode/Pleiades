﻿using System.Web;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Utility
{
    public static class HttpContextExtensions
    {
        public const string _key = "AggregateUserCache";

        public static AggregateUser RetreiveAggregateUserFromContext(this HttpContextBase context)
        {
            return context.Items[_key] as AggregateUser;
        }

        public static AggregateUser RetreiveAggregateUserFromContext(this HttpContext context)
        {
            return context.Items[_key] as AggregateUser;
        }

        public static void StoreAggregateUserInContext(this HttpContextBase context, AggregateUser user)
        {
            context.Items[_key] = user;
        }

    }
}
