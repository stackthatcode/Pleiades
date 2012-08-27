using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Providers
{
    public class PfMembershipRepositoryShim
    {
        public static Func<IMembershipRepository> RepositoryFactory { get; set; }
    }
}
