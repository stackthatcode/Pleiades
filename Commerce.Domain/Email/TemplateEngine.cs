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
        private readonly ITemplateLocator _templateLocator;

        private const string ContentPlaceholder = "@@Content";

        public TemplateEngine(ITemplateLocator templateLocator)
        {
            _templateLocator = templateLocator;
        }

        public string Render<T>(T model, TemplateIdentifier templateIdentifier, bool useMasterTemplate = false)
        {
            var bodyTemplate = _templateLocator.Retreive(templateIdentifier);
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

            if (useMasterTemplate)
            {
                var masterTemplate = _templateLocator.Retreive(TemplateIdentifier.MasterTemplate);
                var bodyWithMasterTemplate = masterTemplate.Replace(ContentPlaceholder, bodyContent);
                return bodyWithMasterTemplate;
            }
            else
            {
                return bodyContent;
            }
        }
    };
}
