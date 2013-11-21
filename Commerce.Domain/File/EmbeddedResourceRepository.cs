using System.IO;
using System.Reflection;
using Commerce.Application.Email;

namespace Commerce.Application.File
{
    public class EmbeddedResourceRepository : IEmbeddedResourceRepository
    {
        public string RetrieveTextFileResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
