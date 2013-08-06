﻿using System;
using System.Configuration;
using System.IO;
using Commerce.Persist.Database;
using Pleiades.Injection;
using Pleiades.Utility;
using Commerce.Persist.Concrete;
using Commerce.Initializer.Builders;

namespace Commerce.Initializer
{
    class Program
    {
        static PushMarketContext DbContext { get; set; }

        static void Main(string[] args)
        {
            DbContext = new PushMarketContext();

            Console.WriteLine("\nPushMarket Commerce Version v1.0 Prototype Initializer");
            Console.WriteLine("Push Cloud Global - All Rights Reserved");
            Console.WriteLine("...");

            // Database Destruction, Resource Directory too
            DestroyDatabase();
            DestroyResourceFiles();
            CreateDatabase();

            // Components Initialization
            var serviceLocator = AutofacBootstrap.CreateContainer();

            // Application Data Initialization
            RootUserBuilder.CreateTheSupremeUser(serviceLocator);
            CategoryBuilder.Populate(serviceLocator);
            SizeBuilder.Populate(serviceLocator);
            BrandBuilder.Populate(serviceLocator);
            ColorBuilder.Populate(serviceLocator);
            ProductBuilder.Populate(serviceLocator);
            ShippingMethodsBuilder.Populate(serviceLocator);
            StateTaxBuilder.Populate(serviceLocator);

            //DbContext.
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
            DbContext.MasterDatabaseCreate();

            // Build Database
            Console.WriteLine("Creating Database for Pleiades");
        }
    }
}