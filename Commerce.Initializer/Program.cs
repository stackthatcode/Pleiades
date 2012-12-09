using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Pleiades.Data;
using Pleiades.Injection;
using Pleiades.Utility;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
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
            // Components - leverage Commerce.WebUI
            ServiceLocator = AutofacBootstrap.CreateContainer();
            
            // Data
            InitializeDatabase();

            // Application Initialization
            CreateTheSupremeUser();
        }

        public static void InitializeDatabase()
        {
            DbContext = new PleiadesContext();
            if (!DbContext.Database.Exists())
            {
                DbContext.Database.Create();

                // Build Database
                Console.WriteLine("Creating Database for Pleiades");
            }
        }

        public static void CreateTheSupremeUser()
        {
            var userService = ServiceLocator.Resolve<IAggregateUserService>();
            var userRepository = ServiceLocator.Resolve<IAggregateUserRepository>();
            var users = userRepository.Retreive(new List<UserRole>() { UserRole.Supreme });

            if (users.ToList().Count() < 1)
            {
                var membershipService = ServiceLocator.Resolve<IMembershipService>();
                var membershipUserName = membershipService.GetUserNameByEmail("aleksjones@gmail.com");
                if (membershipUserName != null)
                {
                    membershipService.DeleteUser(membershipUserName);
                }

                var identityuser1 = new CreateOrModifyIdentityRequest
                {
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.NotApplicable,
                    UserRole = UserRole.Supreme,
                    FirstName = "Master",
                    LastName = "Pleiades",
                };

                var membershipuser1 = new CreateNewMembershipUserRequest
                {
                    Email = "aleksjones@gmail.com",
                    Password = "123456",
                    IsApproved = true,
                    PasswordQuestion = "First Karate Teacher",
                    PasswordAnswer = "Sugiyama",
                };

                PleiadesMembershipCreateStatus outstatus1;
                userService.Create(membershipuser1, identityuser1, out outstatus1);

                if (outstatus1 != PleiadesMembershipCreateStatus.Success)
                {
                    throw new Exception("Failed to create Supreme User");
                }
            }
        }
    }
}