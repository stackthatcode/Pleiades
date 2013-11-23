namespace Commerce.Application.File
{
    public interface IEmbeddedResourceRepository
    {
        string RetrieveTextFile(string resourceName);
    }
}