using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Security;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.UnitTests.Identity.Execution
{
    public class StubSecurityContext : IIdentityUserContext, IIdentityRequirementsContext, ISecurityContext
    {
        public StubSecurityContext()
        {
            this.ExecutionStateValid = true;
        }

        public IdentityUser IdentityUser { get; set; }
        public IdentityRequirements IdentityRequirements { get; set; }
        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool ExecutionStateValid { get; set; }
    }
}