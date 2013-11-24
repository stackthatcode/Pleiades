namespace Commerce.Application.Email
{
    public interface IEmailConfigAdapter
    {
        string SmtpHost { get; set; }
        string SmtpUserName { get; set; }
        string SmtpPassword { get; set; }
        string SmtpPort { get; set; }

        string CustomerServiceEmail { get; set; }
        string SystemEmail { get; set; }
        string TemplateDirectory { get; set; }

        string ServerSideMockEnabled { get; set; }
        string MockServiceOutputDirectory { get; set; }
    }
}
