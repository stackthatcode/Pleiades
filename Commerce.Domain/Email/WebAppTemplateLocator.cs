using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commerce.Application.Email
{
    public class WebAppTemplateLocator : ITemplateLocator
    {
        private readonly Dictionary<TemplateIdentifier, string>
            _templateMap =
                new Dictionary<TemplateIdentifier, string>
                {
                    { TemplateIdentifier.AdminOrderReceived, @"Admin\OrderReceived.cshtml" },
                    { TemplateIdentifier.AdminOrderItemsRefunded, @"Admin\OrderItemsRefunded.cshtml" },
                    { TemplateIdentifier.AdminOrderItemsShipped, @"Admin\OrderItemsShipped.cshtml" },
                    { TemplateIdentifier.AdminSystemError, @"Admin\SystemError.cshtml" },

                    { TemplateIdentifier.CustomerOrderReceived, @"Customer\OrderReceived.cshtml" },
                    { TemplateIdentifier.CustomerOrderItemsRefunded, @"Customer\OrderItemsRefunded.cshtml" },
                    { TemplateIdentifier.CustomerOrderItemsShipped, @"Customer\OrderItemsShipped.cshtml" },

                    { TemplateIdentifier.MasterTemplate, @".\Shared\MasterTemplate.txt" },
                };

        private readonly IEmailConfigAdapter _emailConfigAdapter;

        public WebAppTemplateLocator(IEmailConfigAdapter emailConfigAdapter)
        {
            _emailConfigAdapter = emailConfigAdapter;
        }

        public string Retreive(TemplateIdentifier templateIdentifier)
        {
            var path = Path.Combine(_emailConfigAdapter.TemplateDirectory, _templateMap[templateIdentifier]);
            var template = System.IO.File.ReadAllLines(path);

            var parsedTemplate = template[0].Contains("@model") ? template.Skip(1).ToList() : template.ToList();
            var output = string.Join(Environment.NewLine, parsedTemplate);
            return output;
        }
    }
}
