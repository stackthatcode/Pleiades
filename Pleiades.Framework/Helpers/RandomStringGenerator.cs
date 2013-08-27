using System;
using System.Text;

namespace Pleiades.Application.Helpers
{
    public class RandomStringGenerator
    {
        private static Random random = new Random((int)DateTime.Now.Ticks); //thanks to McAden

        public static string Make(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
