using System.Collections.Generic;

namespace Pleiades.Application.Data
{
    public interface IPagedModel<T>
    {
        int CurrentPageNumber { get; set; }
        int ItemsPerPage { get; set; }
        int TotalItems { get; }
        IList<int> PageNumbers { get; }
    }
}
