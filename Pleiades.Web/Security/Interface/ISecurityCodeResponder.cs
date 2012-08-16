﻿using System.Web.Mvc;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Web.Security.Interface
{
    /// <summary>
    /// Interface for responding to SecurityResponseCode's with HTTP postback
    /// </summary>
    public interface ISecurityCodeResponder
    {
        void ProcessSecurityCode(SecurityResponseCode securityResponseCode, AuthorizationContext filterContext);
    }
}