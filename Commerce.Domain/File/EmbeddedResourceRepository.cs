using System.IO;
using System.Reflection;
using Commerce.Application.Email;

namespace Commerce.Application.File
{
    public class EmbeddedResourceRepository : IEmbeddedResourceRepository
    {
        public string RetrieveTextFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
