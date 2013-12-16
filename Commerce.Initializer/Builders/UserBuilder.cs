using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.App.Logging;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;

namespace ArtOfGroundFighting.Initializer.Builders
{
    public class UserBuilder : IBuilder
    {
        private readonly IAggregateUserService _userService;
        private readonly IReadOnlyAggregateUserRepository _userRepository;
        private readonly IPfMembershipService _membershipService;

        public UserBuilder(
            IAggregateUserService userService, IReadOnlyAggregateUserRepository userRepository,
            IPfMembershipService membershipService)
        {
            _userService = userService;
            _userRepository = userRepository;
            _membershipService = membershipService;
        }

        public void Run()
        {
            LoggerSingleton.Get().Info("Create the default Root User");
            
            var users = _userRepository.Retreive(new List<UserRole>() { UserRole.Root });

            if (!users.ToList().Any())
            {
                AddRootUser( "aleksjones@gmail.com", "Aleksandr", "Jones", "123456", "First Karate Teacher", "Sugiyama");
                AddRootUser("jeremysimon@me.com", "Jeremy", "Simon", "123456", "First Spiritual Teacher", "Murphy");
            }
        }

        private void AddRootUser(string email, string firstName, string lastName, string password, string passwordQuestion,
                                 string passwordAnswer)
        {
            var membershipUser = _membershipService.GetUserByEmail(email);
            if (membershipUser != null)
            {
                _membershipService.DeleteUser(membershipUser.UserName);
            }

            var identityuser1 = new IdentityProfileChange
                {
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.NotApplicable,
                    UserRole = UserRole.Root,
                    FirstName = firstName,
                    LastName = lastName,
                };

            var membershipuser1 = new PfCreateNewMembershipUserRequest
                {
                    Email = email,
                    Password = password,
                    IsApproved = true,
                    PasswordQuestion = passwordQuestion,
                    PasswordAnswer = passwordAnswer,
                };

            string outstatus1;
            var user = _userService.Create(membershipuser1, identityuser1, out outstatus1);

            if (user == null)
            {
                throw new Exception("Failed to create Root User: " + outstatus1);
            }
        }
    }
}