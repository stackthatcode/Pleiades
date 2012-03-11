using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.ServiceHostApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var metadataPub = new MetadataPublisher<ISimpleService, SimpleService>();
            metadataPub.Do();

            var host = new ServiceHostManager<SimpleService>();
            host.Launch();
        }
    }
}
