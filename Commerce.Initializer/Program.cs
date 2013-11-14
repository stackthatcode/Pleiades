using System;
using System.Configuration;
using System.IO;
using Commerce.Initializer.Builders.Products;
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
        private static readonly PushMarketContext DbContext;
        private static readonly IContainerAdapter ServiceLocator;

        static Program()
        {
            ServiceLocator = AutofacBootstrap.CreateContainer();
            DbContext = new PushMarketContext();
        }

        static void CreateAndRunBuilder<T>() where T : IBuilder
        {
            var builder = ServiceLocator.Resolve<T>();
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
            LoggerSingleton.Get().Info(ConfigurationManager.AppSettings["ApplicationName"]);
            LoggerSingleton.Get().Info(ConfigurationManager.AppSettings["CompanyName"]);
            LoggerSingleton.Get().Info("...");

            // Database Destruction, Resource Directory too
            DestroyDatabase();
            DestroyResourceFiles();
            CreateDatabase();

            // Components Initialization

            // Application Data Initialization
            LoggerSingleton.Get().Info("Seeding Pushmarket data - Starting...");

            CreateAndRunBuilder<UserBuilder>();
            CreateAndRunBuilder<CategoryBuilder>();
            CreateAndRunBuilder<SizeBuilder>();
            CreateAndRunBuilder<BrandBuilder>();
            CreateAndRunBuilder<ColorBuilder>();

            CreateAndRunBuilder<TatamiEstiloBuilder>();
            CreateAndRunBuilder<BullTerrierMushinBuilder>();
            CreateAndRunBuilder<BullTerrierSuperStarBuilder>();
            CreateAndRunBuilder<BullTerrierZebraBuilder>();
            CreateAndRunBuilder<BullTerrierMastersBuilder>();
            CreateAndRunBuilder<HayabusaHeadGearBuilder>();
            CreateAndRunBuilder<JiuJitsuDummyBuilder>();
            CreateAndRunBuilder<ShockDoctorMouthGuardBuilderCase>();
            CreateAndRunBuilder<ShockDoctorGelNanoBuilder>();
            CreateAndRunBuilder<TapoutMouthguardCaseBuilder>();
                        
            CreateAndRunBuilder<ShippingMethodsBuilder>();
            CreateAndRunBuilder<StateTaxBuilder>();
            CreateAndRunBuilder<AnalyticsBuilder>();

            LoggerSingleton.Get().Info("Seeding Pushmarket data - Complete!");
        }

        public static void DestroyDatabase()
        {
            LoggerSingleton.Get().Info("Attemping to Delete Database...");
            DbContext.Database.Delete();
            LoggerSingleton.Get().Info("Database Deleted");
        }

        public static void DestroyResourceFiles()
        {
            // Clean-out the Resource Directory
            var resourceDirectory = ConfigurationManager.AppSettings["ResourceStorage"];
            LoggerSingleton.Get().Info("Cleaning out Resource Directory: " + resourceDirectory + "...");
            var directoryInfo = new DirectoryInfo(resourceDirectory);
            directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).ForEach(x => x.Delete());
            directoryInfo.GetDirectories().ForEach(x => x.Delete());
            LoggerSingleton.Get().Info("Resource Directory Cleaned");
        }

        public static void CreateDatabase()
        {
            // Build Database
            LoggerSingleton.Get().Info("Creating Pushmarket Database...");
            DbContext.MasterDatabaseCreate();
            LoggerSingleton.Get().Info("Pushmarket Database Created");
        }
    }
}