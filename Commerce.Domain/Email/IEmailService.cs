namespace Commerce.Application.Email
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }
}
