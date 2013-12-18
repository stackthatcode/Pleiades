using Autofac;
using NUnit.Framework;
using Pleiades.App.Data;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.IntegrationTests.Services
{
    [TestFixture]
    public class MembershipRepositoryTests : FixtureBase
    {        
        [Test]
        public void Create_MembershipUser_Pass()
        {
            var request = new PfCreateNewMembershipUserRequest()
                    {
                        Email = "aleksjones1@gmail.com",
                        IsApproved = true,
                        Password = "password123",
                        PasswordAnswer = "You",
                        PasswordQuestion = "Who dat?",
                    };

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                
                PfMembershipCreateStatus statusResponse;
                var newUser = service.CreateUser(request, out statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(statusResponse, PfMembershipCreateStatus.Success);
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {                
                var repository = lifetime.Resolve<IMembershipReadOnlyRepository>();
                var testUser = repository.GetUserByEmail(request.Email);
                Assert.AreEqual(request.Email, testUser.Email);
                Assert.AreEqual(request.IsApproved, testUser.IsApproved);
            }
        }

        [Test]
        public void Create_Approved_User_And_Authenticate_Pass()
        {
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones2@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                var newUser = service.CreateUser(request, out statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(statusResponse, PfMembershipCreateStatus.Success);
            }

            using (var lifetime2 = TestContainer.LifetimeScope())
            {
                var service = lifetime2.Resolve<IPfMembershipService>();
                var result = service.ValidateUserByEmailAddr(request.Email, request.Password);
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void Create_Approved_User_And_Authenticate_WithBadPassword_Should_Fail()
        {
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones3@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                var newUser = service.CreateUser(request, out statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(statusResponse, PfMembershipCreateStatus.Success);
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var result = service.ValidateUserByEmailAddr(request.Email, "bullshit password");
                Assert.IsNull(result);
            }
        }

        [Test]
        public void Create_Disapproved_User_And_Authenticate_WithGoodPassword_Should_Fail()
        {
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones4@gmail.com",
                IsApproved = false,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                var newUser = service.CreateUser(request, out statusResponse);
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(statusResponse, PfMembershipCreateStatus.Success);
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var result = service.ValidateUserByEmailAddr(request.Email, request.Password);
                Assert.IsNull(result);
            }
        }

        [Test]
        public void Create_User_And_Reset_Password_Fail_To_Authenticate_Then_Reset_Then_Authenticate_And_Pass()
        {
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones5@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };
            PfMembershipUser newUser;

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                newUser = service.CreateUser(request, out statusResponse);
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);
                unitOfWork.SaveChanges();
            }

            string resetPwd;
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                // Reset the Password
                PfCredentialsChangeStatus status;
                resetPwd = service.ResetPassword(newUser.UserName, null, true, out status);
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                // Try authenticating with valid credentials
                var result = service.ValidateUserByEmailAddr(request.Email, resetPwd);
                unitOfWork.SaveChanges();
                Assert.IsNotNull(result);

                // Try authenticating with invalid credentials
                var result2 = service.ValidateUserByEmailAddr(request.Email, "fgghjkgaaewgw");
                unitOfWork.SaveChanges();
                Assert.IsNull(result2);

                service.ChangePassword(newUser.UserName, resetPwd, "password1", true);
                unitOfWork.SaveChanges();                
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                // Now, use the reset password to change the Password
                var result3 = service.ValidateUserByEmailAddr(request.Email, "password1");
                Assert.IsNotNull(result3);
            }
        }

        [Test]
        public void Disapprove_User_And_Validate_Fails_Reapprove_And_Validate_Succeeds()
        {
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones6@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };
            PfMembershipUser newUser;

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                newUser = service.CreateUser(request, out statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

                // Disapprove User
                service.SetUserApproval(newUser.UserName, false);
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                // Try to Authenticate - should fail
                var result1 = service.ValidateUserByEmailAddr(request.Email, "password123");
                Assert.IsNull(result1);

                // Reapprove USer
                service.SetUserApproval(newUser.UserName, true);
                unitOfWork.SaveChanges();
            }


            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                
                // Try to Authenticate - should succeed
                var result2 = service.ValidateUserByEmailAddr(request.Email, "password123");
                Assert.IsNotNull(result2);
            }
        }

        [Test]
        public void Failed_Authentication_Locks_User_Out_And_Unlock_User_Enables_Validation()
        {
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones7@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };
            PfMembershipUser newUser;

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                var service = lifetime.Resolve<IPfMembershipService>();
               
                PfMembershipCreateStatus statusResponse;
                newUser = service.CreateUser(request, out statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                var service = lifetime.Resolve<IPfMembershipService>();

                // Get user
                for (int i = 0; i < 10; i++)
                {
                    service.ValidateUserByEmailAddr(request.Email, "wrongpassword");
                    unitOfWork.SaveChanges();
                }
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();            // Should be locked out
                var service = lifetime.Resolve<IPfMembershipService>();
                var result1 = service.ValidateUserByEmailAddr(request.Email, "password123");

                Assert.IsNull(result1);
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                var service = lifetime.Resolve<IPfMembershipService>();
                // Unlock the User
                service.UnlockUser(newUser.UserName);
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                // Should be able to authenticated
                var result2 = service.ValidateUserByEmailAddr(request.Email, "password123");
                Assert.IsNotNull(result2);
            }
        }

        [Test]
        public void Change_User_Email_And_Validate_Successfully()
        {
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones8@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };
            PfMembershipUser newUser;

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                newUser = service.CreateUser(request, out statusResponse);
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>(); // Change the password
                service.ChangeEmailAddress(newUser.UserName, null, "bob4444@bob.com", true);
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>(); // Change the password
                
                // Try authenticating with valid credentials
                var result = service.ValidateUserByEmailAddr("bob4444@bob.com", "password123");
                unitOfWork.SaveChanges();
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void Change_User_Email_Address_To_Another_Users_Email_Address()
        {
            var request1 = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones9@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };
            PfMembershipUser newUser1;

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                newUser1 = service.CreateUser(request1, out statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                var request2 = new PfCreateNewMembershipUserRequest()
                    {
                        Email = "bob999@gmail.com",
                        IsApproved = true,
                        Password = "password123",
                        PasswordAnswer = "1123",
                        PasswordQuestion = "fdgdfgdgf",
                    };
                PfMembershipCreateStatus statusResponse2;
                var newUser2 = service.CreateUser(request2, out statusResponse2);
                unitOfWork.SaveChanges();
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse2);
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();            // Should throw an Exception
                var result = service.ChangeEmailAddress(newUser1.UserName, null, "bob999@gmail.com", true);
                Assert.That(result, Is.EqualTo(PfCredentialsChangeStatus.EmailAddressAlreadyTaken));
            }
        }

        [Test]
        public void Reset_Password_With_Question_And_Answer_Test()
        {
            var request1 = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones10@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };
            PfMembershipUser newUser1;

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                newUser1 = service.CreateUser(request1, out statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);
            }

            string resetPwd;
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                var question = service.PasswordQuestion(newUser1.UserName);
                Assert.AreEqual(question, request1.PasswordQuestion);

                // Try Reseting
                PfCredentialsChangeStatus status;
                resetPwd = service.ResetPassword(newUser1.UserName, request1.PasswordAnswer, true, out status);
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                
                // Try authenticating with valid credentials
                var result = service.ValidateUserByEmailAddr(newUser1.Email, resetPwd);
                unitOfWork.SaveChanges();
                Assert.IsNotNull(result);
            }
        }

        [Test]
        [ExpectedException()]
        public void Reset_Password_With_Wrong_And_Answer_And_FAIL()
        {
            var request1 = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones11@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };
            PfMembershipUser newUser1;

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                newUser1 = service.CreateUser(request1, out statusResponse);
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);
            }


            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                // Reset the Password with WRONG answer
                PfCredentialsChangeStatus status;
                var resetPwd = service.ResetPassword(newUser1.UserName, null, false, out status);
                unitOfWork.SaveChanges();

                // Should've *thrown* an Exception
            }
        }

        [Test]
        public void ChangePasswordQuestionAndAnswerTest()
        {
            var request1 = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones12@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };
            PfMembershipUser newUser1;
            
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                PfMembershipCreateStatus statusResponse;
                newUser1 = service.CreateUser(request1, out statusResponse);
                unitOfWork.SaveChanges();
                Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                // Change the question and answer
                service.ChangePasswordQuestionAndAnswer(newUser1.UserName, "password123", "New Question", "New Answer", false);
                unitOfWork.SaveChanges();
            }

            string resetPwd;
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                // Test the question
                var question = service.PasswordQuestion(newUser1.UserName);
                unitOfWork.SaveChanges();
                Assert.AreEqual(question, "New Question");

                // Try Reseting
                PfCredentialsChangeStatus status;
                resetPwd = service.ResetPassword(newUser1.UserName, "New Answer", true, out status);
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IPfMembershipService>();
                
                // Try authenticating with valid credentials
                var result = service.ValidateUserByEmailAddr(request1.Email, resetPwd);
                Assert.IsNotNull(result);
            }
        }
    }
}
