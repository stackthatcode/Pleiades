using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Pleiades.Commerce.ServiceApp
{
    [ServiceContract(Namespace = "http://aleksjones.com")]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    [ServiceContract(Namespace = "http://aleksjones.com")]
    public interface IService1Enhanced : IService1
    {
        [OperationContract]
        string GetSpecial(double value);
    }

    [DataContract(Namespace = "http://aleksjones.com")]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
