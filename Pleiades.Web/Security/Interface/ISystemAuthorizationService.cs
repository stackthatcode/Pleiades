﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using  Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Interface
{
    public interface ISystemAuthorizationService
    {
        SecurityResponseCode Authorize(DomainUser user, SecurityRequirementsContext context);
    }
}
