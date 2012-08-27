using Pleiades.Execution;

namespace Pleiades.Security
{
    public interface ISecurityContext : IStepContext
    {
        SecurityResponseCode SecurityResponseCode { get; set; }
    }
}
