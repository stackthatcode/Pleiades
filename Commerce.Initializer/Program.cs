using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Pleiades.Data;
using Pleiades.Injection;
using Pleiades.Utility;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Initializer.Builders;
using Commerce.Persist;
using Commerce.Persist.Security;
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
            Console.WriteLine("Push Global LLC - All Rights Reserved");
            Console.WriteLine("...");
            // Database Destruction and Creation
            DestroyDatabase();
            CreateDatabase();

            // Components Initialization
            var serviceLocator = AutofacBootstrap.CreateContainer();

            // Application Data Initialization
            RootUserBuilder.CreateTheSupremeUser(serviceLocator);
            CategoryBuilder.EmptyAndRepopulate(serviceLocator);
            SizeBuilder.EmptyAndRepopulate(serviceLocator);
        }

        public static void DestroyDatabase()
        {
            DbContext.Database.Delete();
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