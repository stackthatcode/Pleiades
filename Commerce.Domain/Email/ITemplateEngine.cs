﻿namespace Commerce.Application.Email
{
    public interface ITemplateEngine
    {
        string Render<T>(T model, TemplateIdentifier templateIdentifier, bool useMasterTemplate = false);
    }
}
