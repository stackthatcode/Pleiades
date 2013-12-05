namespace Pleiades.App.Utility
{
    public static class StringExtensions
    {
        public static string TruncateAfter(this string input, int length)
        {
            return input.Length <= length ? input : input.Substring(0, length);
        }
    }
}