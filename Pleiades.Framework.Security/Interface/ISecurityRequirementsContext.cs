using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.Identity.Interface
{
    public interface ISecurityRequirementsContext
    {
        DomainUser User { get; set; }

        // Hey, all this stuff in the Domain User object... hmmm....
        //int DomainUserId { get; set; }
        //AccountLevel UserAccountLevel { get; set; }
        //AccountStatus UserAccountStatus { get; set; }
        //UserRole UserRole { get; set; }


        int ResourceOwnerDomainUserId { get; set; }
        AuthorizationZone AuthorizationZone { get; set; }
        AccountLevel AccountLevelRestriction { get; set; }
        bool PaymentArea { get; set; }
    }
}
