using System;
using Pleiades.Framework.MembershipProvider.Interface;

namespace Pleiades.Framework.MembershipProvider.Concrete
{
    /// <summary>
    /// Enables injection of IMembershipRepository implementation into the MembershipProvider, which controls the construction
    /// </summary>
    public class RepositoryShim
    {
        public static Func<IMembershipRepository> GetInstance { get; set; }
    }
}