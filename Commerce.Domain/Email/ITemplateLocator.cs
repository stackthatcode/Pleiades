namespace Commerce.Application.Email
{
    public interface ITemplateLocator
    {
        string Retreive(TemplateIdentifier templateIdentifier);
    }
}
