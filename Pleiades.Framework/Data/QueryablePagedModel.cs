using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Framework.Data;

namespace Pleiades.Framework.Web.Common
{
    /// <summary>
    /// Abstracts paging and filtering operations for View Models.  
    /// Simply construct, set the Items, Items Per Page and it's ready to go!
    /// </summary>
    public class PagedModel<T> : IPagedModel
    {
        protected IQueryable<T> items;
        protected int cachedItemCount = 0;

        public PagedModel(IQueryable<T> items)
        {
            this.items = items;
            this.CurrentPageNumber = 1;
            this.ItemsPerPage = 10;

            this.cachedItemCount = items.Count();
        }

        public int CurrentPageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalItems
        {
            get
            {
                return cachedItemCount;
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
            this.CheckConstraints();
            return PageItems(CurrentPageNumber);
        }

        public IEnumerable<T> PageItems(int PageNumber)
        {
            this.CheckConstraints();
            return this.items.Page<T>(PageNumber, ItemsPerPage);
        }

        private void CheckConstraints()
        {
            if (CurrentPageNumber > TotalPages)
                throw new Exception("CurrentPage is greater than Total Pages");
            if (CurrentPageNumber < 0)
                throw new Exception("CurrentPage is less than zero");
        }

        public IList<int> PageNumbers
        {
            get
            {
                if (ItemsPerPage < 0)
                    throw new Exception("ItemsPerPage  is less than zero");

                var output = new List<int>();
                for (int page = 1; page <= TotalPages; page++)
                {
                    output.Add(page);
                }

                return output;
            }
        }
    }
}
