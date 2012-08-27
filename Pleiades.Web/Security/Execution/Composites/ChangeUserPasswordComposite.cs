using System;
using System.Collections.Generic;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution.Abstract;
using Pleiades.Web.Security.Execution.Steps;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Composites
{
    public class ChangeUserPasswordComposite : OwnerAuthCompositeBase<ChangeUserPasswordContext>
    {
        public ChangeUserPasswordComposite(IGenericContainer container, ChangeUserPasswordStep step) 
            : base(container, step)
        {
        }
    }
}
