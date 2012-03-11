using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Domain.NHibernateFluent
{
    public class NHManagerSingleton
    {
        private static readonly NHManager _NHManager = new NHManager();

        public static NHManager Get
        {
            get
            {
                return _NHManager;
            }
        }
    }
}
