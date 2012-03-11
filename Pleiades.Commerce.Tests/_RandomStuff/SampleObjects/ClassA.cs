using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pleiades.Commerce.UnitTests.FunWithSerialization.SampleObjects
{
    [Serializable]
    public class ClassA
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        // Nice way to shoot oneself in the foot, isn't it?

        // This is visible to Xml Serialized!
        [NonSerialized]
        public string InteralCode;
    
        [XmlIgnore]
        public string SecretNonXmlCode { get; set; }
    }
}
