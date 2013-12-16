using Commerce.Application.Payment;
using NUnit.Framework;

namespace ArtOfGroundFighting.IntegrationTests.StripeClient
{
    [TestFixture]
    public class TestConfiguration
    {
        [Test]
        public void Verify()
        {
            Assert.That(StripeConfiguration.Settings.PublishableKey, Is.EqualTo("pk_test_9RUVhRhHKgJwABqlIpKE4hLB"));
            Assert.That(StripeConfiguration.Settings.SecretKey, Is.EqualTo("sk_test_dXprm3JDq3jO6mgd9wZczhoQ"));
            Assert.That(StripeConfiguration.Settings.StripeUrl, Is.EqualTo("http://stripe.com/v2/testUrl"));
        }
    }
}
