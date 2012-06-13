using Pleiades.Framework.Execution;

namespace Pleiades.Framework.Security
{
    public interface ISecurityContext : IStepContext
    {
        SecurityResponseCode SecurityResponseCode { get; set; }
    }
}
