using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Utilities.General
{
    public static class Linq
    {
        public static void ForEach<T>(this IEnumerable<T> Input, Action<T> Func)
        {
            foreach (T Element in Input)
                Func(Element);
        }

        public static List<TOut> ToCastedList<T, TOut>(this IEnumerable<T> Input)
        {
            return Input.Cast<TOut>().ToList();
        }

        public static string ToCommaDelimitedList<T>(this List<T> input)
        {
            var output = input.Aggregate("", (current, next) => { return current + next.ToString() + ","; });
            output = output != "" ? output.TrimEnd(',') : output;
            return output;
        }

        public static List<T> ToList<T>(this string commaDelimitedList, Func<string, T> translator)
        {
            var output = new List<T>();
            foreach (var item in commaDelimitedList.Split(',').Where(x => x != ""))
                output.Add(translator(item));
            return output;
        }
    }
}
