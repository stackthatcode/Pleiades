using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Pleiades.IntegrationTests.ClientTests
{
    public class InProcFactory
    {
        struct HostRecord
        {
            public HostRecord(ServiceHost host, string address)
            {
                this.Host = host;
                this.Address = new EndpointAddress(address);
            }

            public readonly ServiceHost Host;
            public readonly EndpointAddress Address;
        }

        static readonly Uri BaseAddress = new Uri("net.pipe://localhost/" + Guid.NewGuid().ToString());
        static readonly Binding Binding;
        static Dictionary<Type, HostRecord> m_Hosts = new Dictionary<Type, HostRecord>();

        static InProcFactory()
        {
            var binding = new NetNamedPipeBinding();
            binding.TransactionFlow = true;
            Binding = binding;

            AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs e)
            {
                foreach (HostRecord hostrecord in m_Hosts.Values)
                {
                    hostrecord.Host.Close();
                }
            };
        }

        public static I CreateInstance<S, I>()
                where I : class
                where S : I
        {
            var hostRecord = GetHostRecord<S, I>();
            return ChannelFactory<I>.CreateChannel(Binding, hostRecord.Address);
        }

        static HostRecord GetHostRecord<S, I>() where I : class
                                                where S : I
        {
            HostRecord hostRecord;
            if (m_Hosts.ContainsKey(typeof(S)))
            {
                hostRecord = m_Hosts[typeof(S)];
            }
            else
            {
                var host = new ServiceHost(typeof(S), BaseAddress);
                var address = BaseAddress.ToString() + Guid.NewGuid().ToString();
                hostRecord = new HostRecord(host, address);
                m_Hosts.Add(typeof(S), hostRecord);
                host.AddServiceEndpoint(typeof(I), Binding, address);
                host.Open();
            }
            return hostRecord;
        }
		
        public static void CloseProxy<I>(I proxy) where I : ICommunicationObject
        {
            Debug.Assert(proxy != null);
            proxy.Close();
        }
    }
}
