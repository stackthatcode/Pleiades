using System;
using System.Configuration;
using System.IO;
using Pleiades.Application.Injection;
using Pleiades.Application.Logging;
using Pleiades.Application.Utility;
using Commerce.Application.Database;
using Commerce.Initializer.Builders;
using Pleiades.Web.Logging;

namespace Commerce.Initializer
{
    class Program
    {
        static PushMarketContext DbContext { get; set; }
        private readonly static IContainerAdapter _serviceLocator;

        static Program()
        {
            _serviceLocator = AutofacBootstrap.CreateContainer();            
        }

        static void CreateAndRunBuilder<T>() where T : IBuilder
        {
            var builder = _serviceLocator.Resolve<T>();
            builder.Run();
        }

        static void Main(string[] args)
        {
            try
            {
                // Logger
                LoggerSingleton.Get = NLoggerImpl.RegistrationFactory("Commerce.Initializer");
                LoggerSingleton.Get().Info("PROCESS START:" + DateTime.Now + Environment.NewLine);
                DoStuff();
                throw new Exception("sdfsdflkl!");
            }
            catch (Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
            }
            finally
            {
                LoggerSingleton.Get().Info(Environment.NewLine + "PROCESS FINISH:" + DateTime.Now + Environment.NewLine);                
            }
        }

        public static void DoStuff()
        {
            DbContext = new PushMarketContext();

            LoggerSingleton.Get().Info("PushMarket Commerce Version v1.0 Prototype Initializer");
            LoggerSingleton.Get().Info("Push Global Software - All Rights Reserved");
            LoggerSingleton.Get().Info("...");

            // Database Destruction, Resource Directory too
            DestroyDatabase();
            DestroyResourceFiles();
            CreateDatabase();

            // Components Initialization

            // Application Data Initialization
            LoggerSingleton.Get().Info("Seeding PushMarket data - Starting...");

            CreateAndRunBuilder<UserBuilder>();
            CreateAndRunBuilder<CategoryBuilder>();
            CreateAndRunBuilder<SizeBuilder>();
            CreateAndRunBuilder<BrandBuilder>();
            CreateAndRunBuilder<ColorBuilder>();
            CreateAndRunBuilder<ProductBuilder>();
            CreateAndRunBuilder<ShippingMethodsBuilder>();
            CreateAndRunBuilder<StateTaxBuilder>();

            LoggerSingleton.Get().Info("Seeding PushMarket data - Complete!");
        }

        public static void DestroyDatabase()
        {
            DbContext.Database.Delete();
            LoggerSingleton.Get().Info("Deleted Database");
        }

        public static void DestroyResourceFiles()
        {
            // Clean-out the Resource Directory
            var resourceDirectory = ConfigurationManager.AppSettings["ResourceStorage"];
            LoggerSingleton.Get().Info("Cleaning out Resource Directory: " + resourceDirectory);
            var directoryInfo = new DirectoryInfo(resourceDirectory);
            directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).ForEach(x => x.Delete());
            directoryInfo.GetDirectories().ForEach(x => x.Delete());
        }

        public static void CreateDatabase()
        {
            // Build Database
            LoggerSingleton.Get().Info("Creating PushMarket database");
            DbContext.MasterDatabaseCreate();
        }
    }
}