namespace Pleiades.App.Utility
{
    public static class BoolExtensions
    {
        public static bool ToBoolTryParse(this object input)
        {
            bool output = false;
            if (input == null)
            {
                return false;
            }
            bool.TryParse(input.ToString(), out output);
            return output;
        }
    }
}
