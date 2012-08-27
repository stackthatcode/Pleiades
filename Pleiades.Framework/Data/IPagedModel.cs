using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Data
{
    public interface IPagedModel<T>
    {
        int CurrentPageNumber { get; set; }
        int ItemsPerPage { get; set; }
        int TotalItems { get; }
        IList<int> PageNumbers { get; }
    }
}
