using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Web.Security.Attributes;


namespace Pleiades.Commerce.Web.Security
{
    public abstract class DomainAuthAttribute : AuthorizeAttributeBase
    {
        #region Attribute Properties
        /// <summary>
        /// The AuthorizationZone specifies 
        /// </summary>
        public AuthorizationZone AuthorizationZone { get; set; }

        /// <summary>
        /// The Account Level Restriction identifies the *minimum* Account Level which is allowed authorization
        /// </summary>
        public AccountLevel AccountLevelRestriction { get; set; }

        /// <summary>
        /// Identifies if this is a Payment Area, so delinquent accounts can take care of business
        /// </summary> 
        public bool PaymentArea { get; set; }
        #endregion


        /// <summary>
        /// Local property extracts Attribute Properties
        /// </summary>
        protected SecurityRequirementsContext MakeSecurityRequirementsContext()
        {
            return new SecurityRequirementsContext
            {
                AuthorizationZone = this.AuthorizationZone,
                AccountLevelRestriction = this.AccountLevelRestriction,
                PaymentArea = this.PaymentArea,
            };
        }

    }
}
