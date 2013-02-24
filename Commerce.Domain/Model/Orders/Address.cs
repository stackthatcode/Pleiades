using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Persist.Model.Orders
{
    public class Address
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
    }
}
