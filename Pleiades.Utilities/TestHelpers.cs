using NUnit.Framework;

namespace Pleiades.TestHelpers
{
    public static class TestHelpers
    {
        public static void ShouldEqual<T>(this T actualValue, T expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
