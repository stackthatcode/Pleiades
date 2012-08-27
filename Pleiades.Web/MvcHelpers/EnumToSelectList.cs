using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Pleiades.Helpers;

namespace Pleiades.Web.MvcHelpers
{
    public class EnumToSelectList
    {
        /// <summary>
        /// Translates enumerated type into sequence of SelectListItems
        /// </summary>
        /// <typeparam name="T">Enumerated type</typeparam>
        /// <returns>IEnumerable of ASP.NET MVC SelectListItems</returns>
        public static IEnumerable<SelectListItem> Make<T>() where T : struct, IConvertible
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                yield return 
                    new SelectListItem 
                    { 
                        Text = item.ToString(), 
                        Value = ((int)item).ToString() 
                    };
            }
        }
    }
}