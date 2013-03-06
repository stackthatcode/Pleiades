using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Transactions;
using Pleiades.Data;
using Pleiades.Injection;
using Pleiades.Utility;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Commerce.Persist;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Commerce.Initializer.Builders;
using Commerce.WebUI;
using Commerce.WebUI.Plumbing;

namespace Commerce.Initializer
{
    class Program
    {
        static PleiadesContext DbContext { get; set; }
        static IContainerAdapter ServiceLocator { get; set; }

        static void Main(string[] args)
        {
            DbContext = new PleiadesContext();

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
            CategoryBuilder.EmptyAndRepopulate(serviceLocator);
            SizeBuilder.EmptyAndRepopulate(serviceLocator);
            BrandBuilder.EmptyAndRepopulate(serviceLocator);
            ColorBuilder.EmptyAndRepopulate(serviceLocator);
            ProductBuilder.EmptyAndRepopulate(serviceLocator);
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
            if (!DbContext.Database.Exists())
            {
                DbContext.Database.Create();

                // Build Database
                Console.WriteLine("Creating Database for Pleiades");
            }
        }
    }
}