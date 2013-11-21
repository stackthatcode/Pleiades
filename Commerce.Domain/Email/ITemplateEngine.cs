namespace Commerce.Application.Email
{
    public interface ITemplateEngine
    {
        string Render<T>(T model, string bodyTemplateResource, bool includeSignature = true);
    }
}
