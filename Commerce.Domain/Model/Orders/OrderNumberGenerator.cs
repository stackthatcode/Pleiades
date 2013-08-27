using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Application.Model.Orders
{
    public class OrderNumberGenerator
    {
        private static Random _random = new Random();
        private const string leadingChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string trailerChars = "0123456789";

        private const int leadingCharsLength = 3;
        private const int trailerNumbersLength = 5;

        public static string Next()
        {
            var leadingId = 
                new string(
                    Enumerable.Repeat(leadingChars, leadingCharsLength)
                      .Select(s => s[_random.Next(s.Length)])
                      .ToArray());

            var trailingId =
                new string(
                    Enumerable.Repeat(trailerChars, trailerNumbersLength)
                      .Select(s => s[_random.Next(s.Length)])
                      .ToArray());

            return leadingId + "-" + trailingId;
        }
    }
}
