using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Identity.Interface;

namespace Pleiades.Framework.Identity.Factories
{
    public class OwnerAuthorizedStepFactory<TContext> : 
            InsertedStepFactory<TContext, SimpleOwnerAuthorizationStep<TContext>>
            where TContext : IOwnerAuthorizationContext
    {
        public OwnerAuthorizedStepFactory(IContainer container)
            : base(container)
        {
        }
    }
}