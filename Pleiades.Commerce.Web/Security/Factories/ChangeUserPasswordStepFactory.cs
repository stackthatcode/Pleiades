using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Factories;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security.Factories
{
    public class ChangeUserPasswordStepFactory : OwnerAuthorizedStepFactory<ChangeUserPasswordContext>
    {
        public ChangeUserPasswordStepFactory(IGenericContainer container) : base(container)
        {
        }
    }
}
