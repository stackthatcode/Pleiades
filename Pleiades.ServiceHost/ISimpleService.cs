using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace Pleiades.ServiceHostApp
{
    [ServiceContract]
    public interface ISimpleService
    {
        [OperationContract]
        string SimpleMethod(string msg);
    }
}
