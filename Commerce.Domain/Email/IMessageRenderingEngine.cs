namespace Commerce.Application.Email
{
    public interface IMessageRenderingEngine
    {
        string Generate(EmailMessage emailMessage);
    }
}
