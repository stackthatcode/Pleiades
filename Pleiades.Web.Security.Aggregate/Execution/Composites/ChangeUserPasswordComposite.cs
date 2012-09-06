using System;
using System.Collections.Generic;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Composites
{
    public class ChangeUserPasswordComposite : OwnerSecuredCompositeBase<ChangeUserPasswordContext>
    {
        public ChangeUserPasswordComposite(IGenericContainer container, ChangeUserPasswordStep step) 
            : base(container, step)
        {
        }
    }
}
