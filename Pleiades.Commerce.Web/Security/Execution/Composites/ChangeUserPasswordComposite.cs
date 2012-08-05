using System;
using System.Collections.Generic;
using Pleiades.Framework.Injection;
using Pleiades.Commerce.Web.Security.Execution.Abstract;
using Pleiades.Commerce.Web.Security.Execution.Steps;
using Pleiades.Commerce.Web.Security.Model;

namespace Pleiades.Commerce.Web.Security.Execution.Composites
{
    public class ChangeUserPasswordComposite : SimpleOwnerAuthComposite<ChangeUserPasswordContext>
    {
        public ChangeUserPasswordComposite(IGenericContainer container, ChangeUserPasswordStep step) 
            : base(container, step)
        {
        }
    }
}
