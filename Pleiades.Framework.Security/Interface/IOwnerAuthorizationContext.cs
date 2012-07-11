using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Identity.Interface
{
    public interface IOwnerAuthorizationContext : ISecurityContext
    {
        IdentityUser CurrentUserIdentity { get; }
        int? OwnerIdentityId { get; }
    }
}