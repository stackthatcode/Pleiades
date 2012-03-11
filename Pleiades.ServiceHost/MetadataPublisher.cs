using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

namespace Pleiades.ServiceHostApp
{
    public class MetadataPublisher<TContract, TImpl> where TImpl : TContract
    {
        public void Do()
        {
            // Create host & endpoint, using the current configuration
            var host = new ServiceHost(typeof(TImpl)); // Should get URL from config
            var endpoint = host.AddServiceEndpoint(typeof(TContract), new WSHttpBinding(), "SimpleServiceObject");

            Console.WriteLine("Service endpoint {0} contains the following:", endpoint.Name);
            Console.WriteLine("Binding: {0}", endpoint.Binding.ToString());
            Console.WriteLine("Contract: {0}", endpoint.Contract.ToString());
            Console.WriteLine("ListenUri: {0}", endpoint.ListenUri.ToString());
            Console.WriteLine("ListenUriMode: {0}", endpoint.ListenUriMode.ToString());

            // WSDL Exporter
            WsdlExporter exporter = new WsdlExporter();
            exporter.PolicyVersion = PolicyVersion.Policy15;

            // Can pry serially export multiple endpoints
            exporter.ExportEndpoint(endpoint);

            // Get the Metadata Set
            var metadataSet = exporter.GetGeneratedMetadata();

            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter);
            metadataSet.WriteTo(xmlWriter);

            // Enable this line to spit the XML WSDL out to the console
            //Console.WriteLine(stringWriter.ToString());
        }
    }
}
