using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Utilities.General;

namespace Pleiades.Web.Common.PagedModel
{
    /// <summary>
    /// DEPRECATED - TODO: REMOVE
    /// </summary>
    
    /// <summary>
    /// Abstracts paging and filtering operations for View Models.  
    /// Simply construct, set the Items, Items Per Page and it's ready to go!
    /// 
    /// POSSIBLE PROBLEM - may suck performance wise.  Have to see if the Lambda method penetrate into the
    /// ... EF invocation in the repositories.
    /// </summary>
    public class PagedModel<T> : IPagedModel
    {
        protected IEnumerable<T> items;

        public PagedModel(IEnumerable<T> items)
        {
            this.items = items;
            this.CurrentPageNumber = 1;
            this.ItemsPerPage = 10;
        }

        public int CurrentPageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalItems
        {
            get
            {
                return items.Count();
            }
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }

        public IEnumerable<T> CurrentPageItems()
        {
            return PageItems(CurrentPageNumber);
        }

        public IEnumerable<T> PageItems(int PageNumber)
        {
            return this.items.Page<T>(PageNumber, ItemsPerPage);
        }

        public IList<int> PageNumbers
        {
            get
            {
                if (CurrentPageNumber > TotalPages)
                    throw new Exception("CurrentPage is greater than Total Pages");
                if (CurrentPageNumber < 0)
                    throw new Exception("CurrentPage is less than zero");
                if (ItemsPerPage < 0)
                    throw new Exception("ItemsPerPage  is less than zero");

                List<int> output = new List<int>();
                for (int page = 1; page <= TotalPages; page++)
                    output.Add(page);

                return output;
            }
        }
    }
}
