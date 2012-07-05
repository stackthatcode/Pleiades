using System;
using System.Web;
using Pleiades.Commerce.Domain.Entities.Users;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Commerce.Web.Security.Model
{
    public class AggrUserAuthContext : ISecurityContext, IIdentityRequirementsContext, IIdentityUserContext
    {
        public bool ExecutionStateValid { get; set; }

        public HttpContextBase HttpContext { get; set; }
        public AggregateUser AggregateUser { get; set; }        
        public SecurityResponseCode SecurityResponseCode { get; set; }

        public IdentityRequirements IdentityRequirements { get; set; }

        public IdentityUser IdentityUser
        {
            get
            {
                if (AggregateUser == null)
                {
                    return null;
                }
                else
                {
                    return AggregateUser.IdentityUser;
                }
            }
        }
    }
}
