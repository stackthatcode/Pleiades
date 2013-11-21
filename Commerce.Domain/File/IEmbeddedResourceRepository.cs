namespace Commerce.Application.File
{
    public interface IEmbeddedResourceRepository
    {
        string RetrieveTextFileResource(string resourceName);
    }
}