namespace Commerce.Domain.Model.Lists
{
    public static class BrandExtensions
    {
        public static JsonBrand ToJson(this Brand brand)
        {
            return new JsonBrand
            {
                Id = brand.Id,
                Name = brand.Name,
                SEO = brand.SEO,
                SkuCode = brand.SkuCode,
                Description = brand.Description,
                ImageBundleExternalResourceId = brand.ImageBundle.ExternalId,
            };
        }
    }
}
