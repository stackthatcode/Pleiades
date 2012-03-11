using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Web.Common.PagedModel
{
    /// <summary>
    /// DEPRECATED - TODO: REMOVE
    /// </summary>
    public interface IPagedModel
    {
        int CurrentPageNumber { get; set; }
        int ItemsPerPage { get; set; }
        int TotalItems { get; }
        IList<int> PageNumbers { get; }
    }
}
