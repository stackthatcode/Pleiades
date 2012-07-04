using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.MembershipProvider.Interface;

namespace Pleiades.Framework.MembershipProvider.Providers
{
    public class PfMembershipRepositoryShim
    {
        public static Func<IMembershipRepository> RepositoryFactory { get; set; }
    }
}
