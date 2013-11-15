namespace Commerce.Application.Lists.Entities
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
                ImageBundleExternalId = brand.ImageBundle != null ? brand.ImageBundle.ExternalId.ToString() : null,
            };
        }
    }
}
