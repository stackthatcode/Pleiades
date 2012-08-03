using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Commerce.Web.Security.Model
{
    public class ChangeUserPasswordContext : OwnerAuthorizationContextBase
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
