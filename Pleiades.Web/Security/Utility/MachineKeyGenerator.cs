using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Pleiades.Web.Security.KeyGen
{
    public class MachineKeyGenerator
    {
        public static Output Generate(object sender, EventArgs e)
        {
            return new Output
            {
                _128bit = Generate(128),
                _64bit = Generate(64),
                _48bit = Generate(48),
            };
        }

        public static string Generate(int len)
        {
            var sb = new StringBuilder(len);
            var buff = new byte[len / 2];
            var rng = new RNGCryptoServiceProvider();
            
            rng.GetBytes(buff);

            for (int i = 0; i < buff.Length; i++)
            {
                sb.Append(string.Format("{0:X2}", buff[i]));
            }
            return sb.ToString();
        }

        public class Output
        {
            public string _128bit { get; set; }
            public string _64bit { get; set; }
            public string _48bit { get; set; }
        }
    }
}

