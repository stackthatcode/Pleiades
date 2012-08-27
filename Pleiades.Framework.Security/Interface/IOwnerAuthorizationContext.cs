using Pleiades.Web.Security.Model;
using Pleiades.Security;

namespace Pleiades.Web.Security.Interface
{
    public interface IOwnerAuthorizationContext : ISecurityContext
    {
        IdentityUser CurrentUserIdentity { get; }
        int? OwnerIdentityId { get; }
    }
}