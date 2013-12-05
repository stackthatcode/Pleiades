using System;
using System.Collections.Generic;
using System.Linq;

namespace Pleiades.App.Helpers
{
    public static class Linq
    {
        public static void ForEach<T>(this IEnumerable<T> input, Action<T> action)
        {
            foreach (T Element in input)
                action(Element);
        }

        public static List<TOut> ToCastedList<T, TOut>(this IEnumerable<T> input)
        {
            return input.Cast<TOut>().ToList();
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
