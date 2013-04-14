using Commerce.Persist.Model.Email;

namespace Commerce.Persist.Interfaces
{
    public interface IEmailService
    {
        void SendOrderReceived();
        void SendOrderShipped();
    }
}
