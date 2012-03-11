using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NUnit.Framework;
using Pleiades.IntegrationTests.ServiceApp;

namespace Pleiades.IntegrationTests.ClientTests
{
    [TestFixture]
    public class NormalService1ClientTest
    {
        [Test]
        public void TestMethod1()
        {
            // Create the proxy, mane!
            var client = new ServiceApp.Service1EnhancedClient();
            var result = client.GetData(8);
            Assert.AreEqual("You entered: 11", result);
        }

        /*
        [Test]
        public void ChannelFactory()
        {
            var factory = new ChannelFactory<IService1Enhanced>();
            var proxy = factory.CreateChannel();
            var result = proxy.GetData(8);
            Assert.AreEqual("You entered: 11", result);
        }
         */
    }
}
