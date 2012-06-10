using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Framework.Web.Common
{
    public interface IPagedModel
    {
        int CurrentPageNumber { get; set; }
        int ItemsPerPage { get; set; }
        int TotalItems { get; }
        IList<int> PageNumbers { get; }
    }
}
