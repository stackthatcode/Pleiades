using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Pleiades.Commerce.UnitTests.FunWithSerialization
{
    [TestFixture]
    public class Fixture1
    {
        [Test]
        public void XmlSerializer()
        {
            var obj1 = new SampleObjects.ClassA
            {
                Address = "123 Test Street",
                Name = "Joshua Givnes",
                InteralCode = "33333",
                Price = 888,
                Quantity = 7,
                SecretNonXmlCode = "99999",
            };

            var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(SampleObjects.ClassA));
            serializer.Serialize(writer, obj1);

            Debug.WriteLine(writer.ToString());
        }

        [Test]
        public void SoapFormatterDefault()
        {
            var obj1 = new SampleObjects.ClassA
            {
                Address = "123 Test Street",
                Name = "Joshua Givnes",
                InteralCode = "33333",
                Price = 888,
                Quantity = 7,
                SecretNonXmlCode = "99999",
            };

            // Put the results of the Soap Formatter in memory
            var stream = new MemoryStream();
            var serializer = new SoapFormatter();
            serializer.Serialize(stream, obj1);

            // ... and read the results
            var reader = new StreamReader(stream);
            stream.Position = 0;
            var output = reader.ReadToEnd();

            Debug.WriteLine(output);
        }
    }
}
