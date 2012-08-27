using Pleiades.Web.Security.Model;
using Pleiades.Security;

namespace Pleiades.Web.Security.Interface
{
    public interface ISystemAuthorizationContext : ISecurityContext
    {
        IdentityUser CurrentIdentity { get; }
        AuthorizationZone AuthorizationZone { get; }
        AccountLevel AccountLevelRestriction { get; }
        bool IsPaymentArea { get; }
    }
}
