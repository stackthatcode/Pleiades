using System;
using System.Collections.Generic;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Web.Security.Execution.Abstract;
using Pleiades.Framework.Web.Security.Execution.Steps;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Execution.Composites
{
    public class ChangeUserPasswordComposite : SimpleOwnerAuthComposite<ChangeUserPasswordContext>
    {
        public ChangeUserPasswordComposite(IGenericContainer container, ChangeUserPasswordStep step) 
            : base(container, step)
        {
        }
    }
}
