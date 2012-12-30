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
            // Components Initialization
            ServiceLocator = AutofacBootstrap.CreateContainer();
            DbContext = new PleiadesContext();
            
            // Database Destruction and Creation
            DestroyDatabase();
            CreateDatabase();

            // Application Data Initialization
            CategoryBuilder.EmptyAndRepopulate(ServiceLocator);
            RootUserBuilder.CreateTheSupremeUser(ServiceLocator);
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