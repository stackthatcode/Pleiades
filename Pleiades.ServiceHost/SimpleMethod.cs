using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.ServiceHostApp
{
    public class SimpleService : ISimpleService
    {
        public string SimpleMethod(string msg)
        {
            Console.WriteLine("The caller passed in " + msg);
            return "Hello " + msg;
        }
    }
}
