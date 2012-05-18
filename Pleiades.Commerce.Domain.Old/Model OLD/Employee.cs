using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Domain.Old.Model
{
    public class Employee
    {
        public virtual int Id { get; private set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Store Store { get; set; }
    }
}
