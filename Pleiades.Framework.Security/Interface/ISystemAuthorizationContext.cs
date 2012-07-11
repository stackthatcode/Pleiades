using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Identity.Interface
{
    public interface ISystemAuthorizationContext : ISecurityContext
    {
        IdentityUser CurrentIdentity { get; }
        AuthorizationZone AuthorizationZone { get; }
        AccountLevel AccountLevelRestriction { get; }
        bool IsPaymentArea { get; }
    }
}
