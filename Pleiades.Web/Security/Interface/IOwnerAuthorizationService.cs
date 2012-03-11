using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using  Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IOwnerAuthorizationService
    {
        SecurityResponseCode Authorize(DomainUser user, OwnershipContext context);
    }
}
