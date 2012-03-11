using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Pleiades.Utilities.General;

namespace Pleiades.Utilities.TestHelpers.General
{
    public static class TestHelpers
    {
        public static void ShouldEqual<T>(this T actualValue, T expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
