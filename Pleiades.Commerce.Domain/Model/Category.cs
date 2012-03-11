using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Domain.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string ActiveYesNo { get { return Active ? "Yes" : "No"; } }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
    }
}
