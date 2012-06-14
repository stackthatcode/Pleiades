using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Pleiades.Framework.Membership.Model;

namespace Pleiades.Framework.Membership.Interface
{
    public interface IHttpContextUserService
    {
        DomainUser RetrieveUserFromHttpContext(HttpContextBase context);
    }
}