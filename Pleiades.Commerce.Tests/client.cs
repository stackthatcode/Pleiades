﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Runtime.Serialization.ContractNamespaceAttribute("http://aleksjones.com", ClrNamespace="aleksjones.com")]

namespace aleksjones.com
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompositeType", Namespace="http://aleksjones.com")]
    public partial class CompositeType : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private bool BoolValueField;
        
        private string StringValueField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool BoolValue
        {
            get
            {
                return this.BoolValueField;
            }
            set
            {
                this.BoolValueField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue
        {
            get
            {
                return this.StringValueField;
            }
            set
            {
                this.StringValueField = value;
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace="http://aleksjones.com", ConfigurationName="IService1Enhanced")]
public interface IService1Enhanced
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://aleksjones.com/IService1/GetData", ReplyAction="http://aleksjones.com/IService1/GetDataResponse")]
    string GetData(int value);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://aleksjones.com/IService1/GetDataUsingDataContract", ReplyAction="http://aleksjones.com/IService1/GetDataUsingDataContractResponse")]
    aleksjones.com.CompositeType GetDataUsingDataContract(aleksjones.com.CompositeType composite);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://aleksjones.com/IService1Enhanced/GetSpecial", ReplyAction="http://aleksjones.com/IService1Enhanced/GetSpecialResponse")]
    string GetSpecial(double value);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IService1EnhancedChannel : IService1Enhanced, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class Service1EnhancedClient : System.ServiceModel.ClientBase<IService1Enhanced>, IService1Enhanced
{
    
    public Service1EnhancedClient()
    {
    }
    
    public Service1EnhancedClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public Service1EnhancedClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public Service1EnhancedClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public Service1EnhancedClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string GetData(int value)
    {
        return base.Channel.GetData(value);
    }
    
    public aleksjones.com.CompositeType GetDataUsingDataContract(aleksjones.com.CompositeType composite)
    {
        return base.Channel.GetDataUsingDataContract(composite);
    }
    
    public string GetSpecial(double value)
    {
        return base.Channel.GetSpecial(value);
    }
}
