using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Framework.Injection
{
    public interface IGenericBuilder
    {
        void RegisterType<T>();
    }
}
