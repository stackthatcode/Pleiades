using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Pleiades.Commerce.ServiceApp
{
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            var g = 3;

            try  
            {
                return string.Format("You entered: {0}", value + g);
            }
            catch (Exception ex)
            {
                // Question - how does the client proxy know how to deal with generics...?
                throw new FaultException<ExceptionDetail>(new ExceptionDetail(ex), "I love to throw exceptions");
            }
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }

    public class Service1Enhanced : Service1, IService1Enhanced
    {
        public string GetSpecial(double value)
        {
            return value.ToString() + " is like cheese pizza!";
        }
    }
}
