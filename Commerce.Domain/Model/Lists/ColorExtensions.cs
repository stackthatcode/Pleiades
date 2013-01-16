namespace Commerce.Domain.Model.Lists
{
    public static class ColorExtensions
    {
        public static JsonColor ToJson(this Color color)
        {
            return new JsonColor
            {
                Id = color.Id,
                Name = color.Name,
                SEO = color.SEO,
                SkuCode = color.SkuCode,
                ImageBundleExternalId = color.ImageBundle != null ? color.ImageBundle.ExternalId.ToString() : null,
            };
        }
    }
}