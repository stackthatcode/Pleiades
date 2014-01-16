using System;
using System.Configuration;
using System.Diagnostics;
using Pleiades.App.Injection;
using Pleiades.App.Logging;
using Commerce.Application.Database;
using Commerce.Application.File;
using ArtOfGroundFighting.Initializer.Builders;
using ArtOfGroundFighting.Initializer.Builders.Products;

namespace ArtOfGroundFighting.Initializer
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
                LoggerSingleton.Get = NLoggerImpl.RegistrationFactory("Initializer");
                LoggerSingleton.Get().Info("PROCESS START:" + DateTime.Now + Environment.NewLine);
                DoStuff();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                Debug.Write(ex.FullStackTraceDump());
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.FullStackTraceDump());
                LoggerSingleton.Get().Error(ex);
            }
            finally
            {
                LoggerSingleton.Get().Info(Environment.NewLine + "PROCESS FINISH:" + DateTime.Now + Environment.NewLine);                
            }
        }

        public static void DoStuff()
        {
            LoggerSingleton.Get().Info(ConfigurationManager.AppSettings["PushMarketSoftwareName"]);
            LoggerSingleton.Get().Info(ConfigurationManager.AppSettings["PushMarketCompanyName"]);
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

            CreateAndRunBuilder<BullTerrierMushinBuilder>();
            CreateAndRunBuilder<TatamiEstiloBuilder>();
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
            LoggerSingleton.Get().Info("Cleaning out Resource Files");
            
            var fileResourceRepository = ServiceLocator.Resolve<IFileResourceRepository>();
            fileResourceRepository.NuclearDelete(false);

            LoggerSingleton.Get().Info("Resource Files Cleaned");
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