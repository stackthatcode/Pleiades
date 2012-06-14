using System;

namespace Pleiades.Framework.MembershipProvider.Providers
{
    /// <summary>
    /// Enables injection of IMembershipRepository implementation into the MembershipProvider, which controls the construction
    /// </summary>
    public class RepositoryShim
    {
        public static Func<IMembershipRepository> GetInstance { get; set; }
    }
}