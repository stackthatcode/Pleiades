using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Domain.Model
{
    public class LeftTable
    {
        public virtual Guid LeftKey { get; private set; }
        public virtual string Data1 { get; set; }
        public virtual int Data2 { get; set; }
        public virtual float Data3 { get; set; }
    }
}
