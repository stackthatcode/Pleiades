using System;
using System.Configuration;
using System.IO;
using Pleiades.Application.Injection;
using Pleiades.Application.Utility;
using Commerce.Application.Database;
using Commerce.Initializer.Builders;

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
            DbContext = new PushMarketContext();

            Console.WriteLine("\nPushMarket Commerce Version v1.0 Prototype Initializer");
            Console.WriteLine("Push Global Software - All Rights Reserved");
            Console.WriteLine("...");

            // Database Destruction, Resource Directory too
            DestroyDatabase();
            DestroyResourceFiles();
            CreateDatabase();

            // Components Initialization
            
            // Application Data Initialization
            Console.WriteLine("Seeding PushMarket data");

            CreateAndRunBuilder<UserBuilder>();
            CreateAndRunBuilder<CategoryBuilder>();
            CreateAndRunBuilder<SizeBuilder>();
            CreateAndRunBuilder<BrandBuilder>();
            CreateAndRunBuilder<ColorBuilder>();
            CreateAndRunBuilder<ProductBuilder>();
            CreateAndRunBuilder<ShippingMethodsBuilder>();
            CreateAndRunBuilder<StateTaxBuilder>();
        }

        public static void DestroyDatabase()
        {
            DbContext.Database.Delete();
            Console.WriteLine("Deleted Database");
        }

        public static void DestroyResourceFiles()
        {
            // Clean-out the Resource Directory
            var resourceDirectory = ConfigurationManager.AppSettings["ResourceStorage"];
            Console.WriteLine("Cleaning out Resource Directory: " + resourceDirectory);
            var directoryInfo = new DirectoryInfo(resourceDirectory);
            directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).ForEach(x => x.Delete());
            directoryInfo.GetDirectories().ForEach(x => x.Delete());
        }

        public static void CreateDatabase()
        {
            // Build Database
            Console.WriteLine("Creating PushMarket database");
            DbContext.MasterDatabaseCreate();
        }
    }
}