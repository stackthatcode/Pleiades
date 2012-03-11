using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Pleiades.Commerce.WebUI.Plumbing.UI.HtmlHelpers
{
    public static class MetadataExtractorExtension
    {
        public static void GetMetadata<TModel, TProperty>(this HtmlHelper<TModel> html, 
                Expression<Func<TModel, TProperty>> expression)
        {
            // *** Nice sample code here!  How to extract Model Metadata from Express + ViewData
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var expr = ExpressionHelper.GetExpressionText(expression);
            var result = metadata.DisplayName + " " + expr;

            // TODO: ask the metadata for Max Length
        }
    }
}