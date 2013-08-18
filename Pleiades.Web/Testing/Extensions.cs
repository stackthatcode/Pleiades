using System;
using NUnit.Framework;

namespace Pleiades.Web.Testing
{
    public static class Extensions
    {
        public static void ShouldEqual(this object input1, object input2)
        {
            Assert.That(input1, Is.EqualTo(input2));
        }
    }
}
