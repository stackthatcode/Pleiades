using Commerce.Application.File;
using RazorEngine;

namespace Commerce.Application.Email
{
    public class RazorTemplateEngine : ITemplateEngine
    {
        private readonly IEmbeddedResourceRepository _resourceRepository;
        private readonly IEmailConfigAdapter _emailConfigAdapter;
        private const string MasterTemplate = "Commerce.Application.Email.Templates.Shared.MasterTemplate.txt";
        private const string ContentPlaceholder = "@@Content";
        private const string SignaturePlaceholder = "@@Signature";

        public RazorTemplateEngine(
                IEmbeddedResourceRepository resourceRepository, 
                IEmailConfigAdapter emailConfigAdapter)
        {
            _resourceRepository = resourceRepository;
            _emailConfigAdapter = emailConfigAdapter;
        }

        public string Render<T>(T model, string bodyTemplateResource, bool includeSignature = true)
        {
            var bodyTemplate = _resourceRepository.RetrieveTextFileResource(bodyTemplateResource);
            var bodyContent = Razor.Parse(bodyTemplate, model);

            var signature = _resourceRepository.RetrieveTextFileResource(_emailConfigAdapter.SignatureResource);
            var masterTemplate = _resourceRepository.RetrieveTextFileResource(MasterTemplate);

            // TODO: replace this with Razor, if possible...
            var result =
                masterTemplate
                    .Replace(ContentPlaceholder, bodyContent)
                    .Replace(SignaturePlaceholder, signature);
            return result;
        }
    }
}
