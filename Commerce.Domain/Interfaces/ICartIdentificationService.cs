using System;

namespace Commerce.Application.Interfaces
{
    public interface ICartIdentificationService
    {
        Guid ProvisionNewCartId();
        Guid? GetCurrentRequestCardId();
    }
}
