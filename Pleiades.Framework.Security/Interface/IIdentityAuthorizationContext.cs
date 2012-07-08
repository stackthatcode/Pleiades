using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Identity.Interface
{
    public interface IIdentityAuthorizationContext : ISecurityContext
    {
        IdentityUser CurrentUser { get; }
        AuthorizationZone AuthorizationZone { get; }
        AccountLevel AccountLevelRestriction { get; }
        bool IsPaymentArea { get; }
        int? ResourceOwnerId { get; }
    }
}
