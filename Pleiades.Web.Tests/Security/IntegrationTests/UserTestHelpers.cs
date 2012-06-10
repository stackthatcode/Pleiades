using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Framework.Web.Security.Model;
using Gallio.Framework;
using MbUnit.Framework;
using Pleiades.Framework.Helpers;

namespace Pleiades.Web.Tests.Security.IntegrationTests
{
    public class UserTestHelpers
    {
        #region Test Data for Trusted and Admin Users
        public static List<DomainUserCreateRequest> TrustedUsers = new List<DomainUserCreateRequest>()
        {
            new DomainUserCreateRequest()
            {
                Password = "password1",
                Email = "jeff@pleiadestest.com",
                PasswordQuestion = "Password Question 1",
                PasswordAnswer = "Password Answer 1",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Aleks",
                LastName = "Jones"
            },

            new DomainUserCreateRequest()
            {
                Password = "password2",
                Email = "beth@pleiadestest.com",
                PasswordQuestion = "Password Question 2",
                PasswordAnswer = "Password Answer 2",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Beth",
                LastName = "Jones"
            },

            new DomainUserCreateRequest()
            {
                Password = "password3",
                Email = "ralph@pleiadestest.com",
                PasswordQuestion = "Password Question 3",
                PasswordAnswer = "Password Answer 3",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Standard,
                FirstName = "Ralph",
                LastName = "Jones"
            },

            new DomainUserCreateRequest()
            {
                Password = "password4",
                Email = "beth@pleiadestest.com",
                PasswordQuestion = "Password Question 4",
                PasswordAnswer = "Password Answer 4",
                IsApproved = true,
                AccountStatus = AccountStatus.PaymentRequired,
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Standard,
                FirstName = "Big",
                LastName = "Boi"
            },

            new DomainUserCreateRequest()
            {
                Password = "password5",
                Email = "oscar@pleiadestest.com",
                PasswordQuestion = "Password Question 5",
                PasswordAnswer = "Password Answer 5",
                IsApproved = true,
                AccountStatus = AccountStatus.Disabled,
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Oscar",
                LastName = "Trey"
            }
        };

        public static List<DomainUserCreateRequest> AdminUsers = new List<DomainUserCreateRequest>()
        {
            new DomainUserCreateRequest()
            {
                Password = "password1",
                Email = "admin1@pleiadestest.com",
                PasswordQuestion = "Password Question 1",
                PasswordAnswer = "Password Answer 1",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Admin,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Admin1",
                LastName = "Jones"
            },

            new DomainUserCreateRequest()
            {
                Password = "password2",
                Email = "admin2@pleiadestest.com",
                PasswordQuestion = "Password Question 2",
                PasswordAnswer = "Password Answer 2",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Admin,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Admin2",
                LastName = "Jones"
            },

            new DomainUserCreateRequest()
            {
                Password = "password3",
                Email = "admin3@pleiadestest.com",
                PasswordQuestion = "Password Question 3",
                PasswordAnswer = "Password Answer 3",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Admin,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Admin3",
                LastName = "Jones"
            },

            new DomainUserCreateRequest()
            {
                Password = "password4",
                Email = "admin4@pleiadestest.com",
                PasswordQuestion = "Password Question 4",
                PasswordAnswer = "Password Answer 4",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Admin,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Admin4",
                LastName = "Jones"
            },

            new DomainUserCreateRequest()
            {
                Password = "password5",
                Email = "admin5@pleiadestest.com",
                PasswordQuestion = "Password Question 5",
                PasswordAnswer = "Password Answer 5",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Admin,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Admin5",
                LastName = "Jones"
            },
        };

        public static DomainUserCreateRequest 
            AnotherAdmin = new DomainUserCreateRequest()
            {
                Password = "password6",
                Email = "admin6@pleiadestest.com",
                PasswordQuestion = "Password Question 6",
                PasswordAnswer = "Password Answer 6",
                IsApproved = true,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Admin,
                AccountLevel = AccountLevel.Gold,
                FirstName = "Admin6",
                LastName = "Jones"
            };
        #endregion


        public static void ResetTestUser()
        {
            // Delete old
            CleanupTestUser();

            // Create the new
            var service = new DomainUserService();
            MembershipCreateStatus status;

            TrustedUsers.ForEach(x => service.Create(x, out status));
            AdminUsers.ForEach(x => service.Create(x, out status));
        }

        public static void CleanupTestUser()
        {
            // Clear out the old
            var service = new DomainUserService();

            Action<DomainUserCreateRequest> RetrieveDelete = x => 
                {
                    var domainUser = service.RetrieveUserByEmail(x.Email);
                    if (domainUser != null)
                        service.Delete(domainUser);
                };

            TrustedUsers.ForEach(x => RetrieveDelete(x));
            AdminUsers.ForEach(x => RetrieveDelete(x));
            RetrieveDelete(new DomainUserCreateRequest { Email = "bob@bob.com" });
        }
    }
}
