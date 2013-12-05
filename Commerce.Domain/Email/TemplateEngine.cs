using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pleiades.App.Logging;
using RazorEngine;
using RazorEngine.Templating;

namespace Commerce.Application.Email
{
    public class TemplateEngine : ITemplateEngine
    {
        private readonly IEmailConfigAdapter _emailConfigAdapter;

        private const string ContentPlaceholder = "@@Content";
        private const string SignaturePlaceholder = "@@Signature";

        public TemplateEngine(
                IEmailConfigAdapter emailConfigAdapter)
        {
            _emailConfigAdapter = emailConfigAdapter;
        }

        public string Render<T>(T model, TemplateIdentifier templateIdentifier)
        {
            var bodyTemplate = RetreiveTemplate(templateIdentifier);
            var bodyContent = "";

            try
            {
                bodyContent = Razor.Parse(bodyTemplate, model);
            }
            catch (TemplateCompilationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    LoggerSingleton.Get().Error(error.Line + " " + error.ErrorText);
                }
                throw;
            }

            var masterTemplate = RetreiveTemplate(TemplateIdentifier.MasterTemplate);
            var signature = RetreiveTemplate(TemplateIdentifier.EmailSignature);

            // TODO: replace this with Razor Master Page, if possible...
            var result =
                masterTemplate
                    .Replace(ContentPlaceholder, bodyContent)
                    .Replace(SignaturePlaceholder, signature);
            return result;
        }

        private readonly Dictionary<TemplateIdentifier, string> 
            _templateMap = 
                new Dictionary<TemplateIdentifier, string>
                {
                    { TemplateIdentifier.MasterTemplate, @"Shared\MasterTemplate.txt" },
                    { TemplateIdentifier.EmailSignature, @"Shared\EmailSignature.txt" },

                    { TemplateIdentifier.AdminOrderReceived, @"Admin\OrderReceived.cshtml" },
                    { TemplateIdentifier.AdminOrderItemsRefunded, @"Admin\OrderItemsRefunded.cshtml" },
                    { TemplateIdentifier.AdminOrderItemsShipped, @"Admin\OrderItemsShipped.cshtml" },
                    { TemplateIdentifier.AdminSystemError, @"Admin\SystemError.cshtml" },

                    { TemplateIdentifier.CustomerOrderReceived, @"Customer\OrderReceived.cshtml" },
                    { TemplateIdentifier.CustomerOrderItemsRefunded, @"Customer\OrderItemsRefunded.cshtml" },
                    { TemplateIdentifier.CustomerOrderItemsShipped, @"Customer\OrderItemsShipped.cshtml" },
                };

        public string RetreiveTemplate(TemplateIdentifier templateIdentifier)
        {
            var path = Path.Combine(_emailConfigAdapter.TemplateDirectory, _templateMap[templateIdentifier]);
            var template = System.IO.File.ReadAllLines(path);

            var parsedTemplate = template[0].Contains("@model") ? template.Skip(1).ToList() : template.ToList();
            var output = string.Join(Environment.NewLine, parsedTemplate);
            return output;
        }
    };
}
