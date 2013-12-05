using System;

namespace Pleiades.App.Helpers
{
    public static class StringExtensions
    {
        public static string TruncateAfter(this string input, int maxLength)
        {
            return input.Length > maxLength ? input.Substring(0, maxLength) + "..." : input;
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToByte(this string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
    }
}