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
    public class ChannelFactoryService1Tests
    {
        [Test]
        public void Method1()
        {
            var binding = new WSHttpBinding();
            var endpointAddress = new EndpointAddress(new Uri("http://localhost:8001/Service1.svc/Service1"));
            var channelFactory = new ChannelFactory<IService1EnhancedChannel>(binding, endpointAddress);
            var channel = channelFactory.CreateChannel();
            
            var result =  channel.GetData(8);
            Assert.IsNotNull(result);
        }
    }
}
