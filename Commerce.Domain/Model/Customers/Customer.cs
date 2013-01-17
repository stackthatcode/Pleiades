using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Domain.Model.Customers
{
    public class Customer
    {
        // TODO: associate to an Aggregate User

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        // Email...?
    }
}