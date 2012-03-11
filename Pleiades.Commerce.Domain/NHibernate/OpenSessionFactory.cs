using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using Pleiades.Commerce.Domain.Model;

namespace Pleiades.Commerce.Domain.NHibernate
{
    public class OpenSessionFactory
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory Factory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();

                    // OLD CODE ==> configuration.AddAssembly(typeof(SampleProduct).Assembly);
                    configuration.AddAssembly(typeof(OpenSessionFactory).Assembly);                    
                    _sessionFactory = configuration.BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession Make()
        {
            return OpenSessionFactory.Factory.OpenSession();
        }
    }
}
