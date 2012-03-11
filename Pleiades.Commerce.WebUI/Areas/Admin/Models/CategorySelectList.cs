using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Models
{
    public class CategorySelectList
    {
        public static IEnumerable<SelectListItem> Make(string selectedValue)
        {
            yield return new SelectListItem() { Text = "Yes", Value = "True", Selected = (selectedValue == "True") };
            yield return new SelectListItem() { Text = "No", Value = "False", Selected = (selectedValue == "False") };
        }
    }
}
