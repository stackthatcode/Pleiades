using Commerce.Application.Model.Email;

namespace Commerce.Application.Interfaces
{
    public interface IEmailService
    {
        void SendOrderReceived();
        void SendOrderShipped();
    }
}
