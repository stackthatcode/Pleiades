using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Pleiades.Commerce.Domain.NHibernateFluent
{
    public class NHManager
    {
        public Action<ConnectionStringBuilder> ConnectionBuilder = x => x.FromAppSetting("mssql_conn");
        public Func<Assembly> AssemblyFinder = () => typeof(NHManager).Assembly;
        public Func<MsSqlConfiguration> DatabaseChooser = () => MsSqlConfiguration.MsSql2008;

        private ISessionFactory _sessionFactory = null;

        private ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var fluentConfig = MakeFluentConfiguration();
                    return fluentConfig.BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public void CreateDatabase()
        {
            var config = MakeFluentConfiguration();
            config.ExposeConfiguration(x => new SchemaExport(x).Create(true, true));
        }

        public ISession MakeOpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public FluentConfiguration MakeFluentConfiguration()
        {
            return Fluently
                .Configure()
                .Database(MakeMssqlConfiguration())
                .Mappings(m => m.FluentMappings.AddFromAssembly(AssemblyFinder()));
        }

        public MsSqlConfiguration MakeMssqlConfiguration()
        {
            return DatabaseChooser().ConnectionString(ConnectionBuilder);
        }
    }
}
