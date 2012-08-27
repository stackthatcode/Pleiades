using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.WebUI.Areas.Public.Models
{
    public class ProductListModel
    {
        public IEnumerable<SelectListItem> EzList
        {
            get
            {
                yield return new SelectListItem { Text = "Option 1", Value = "1" };
                yield return new SelectListItem { Text = "Option 2", Value = "2" };
                yield return new SelectListItem { Text = "Option 3", Value = "3" };
            }
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public int AccountBalance { get; set; }

        public int SelectedValue { get; set; }
    }
}
