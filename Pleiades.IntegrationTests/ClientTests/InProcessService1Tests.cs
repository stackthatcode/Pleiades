using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pleiades.Commerce.ServiceApp;

namespace Pleiades.IntegrationTests.ClientTests
{
    [TestFixture]
    public class InProcessService1Tests
    {
        [Test]
        public void Method1()
        {
            var proxy = InProcFactory.CreateInstance<Service1Enhanced, IService1Enhanced>();
            var result = proxy.GetData(8);
            Assert.IsNotNull(result);
        }
    }
}
