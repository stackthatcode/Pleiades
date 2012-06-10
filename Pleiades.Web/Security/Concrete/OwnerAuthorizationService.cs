using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Concrete
{
    public class OwnerAuthorizationService : IOwnerAuthorizationService
    {
        public SecurityResponseCode Authorize(DomainUser user, OwnershipContext context)
        {
            if (user.UserRole.IsAdministrator())
                return SecurityResponseCode.Allowed;

            if (user.DomainUserId == context.OwnerDomainUserId)
                return SecurityResponseCode.Allowed;

            return SecurityResponseCode.AccessDenied;
        }
    }
}
