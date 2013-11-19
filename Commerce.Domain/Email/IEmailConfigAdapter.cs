namespace Commerce.Application.Email
{
    public interface IEmailConfigAdapter
    {
        string MockServiceEnabled { get; set; }
        string MockServiceOutputDirectory { get; set; }
        string SmtpHost { get; set; }
        string SmtpUserName { get; set; }
        string SmtpPassword { get; set; }
        string FromAddress { get; set; }
    }
}
