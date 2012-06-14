﻿using System;
using System.Web.Security;
using Pleiades.Framework.Membership.Model;

namespace Pleiades.Framework.Membership.Interface
{
    public interface IFormsAuthenticationService
    {
        void SetAuthCookieForUser(string username, bool persistent);
        void ClearAuthenticationCookie();
    }
}