using System;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.UnitTests.Concrete
{
    [TestFixture]
    public class PfPasswordServiceTests
    {
        [Test]
        public void UpdateFailedPasswordAttempts_Sets_Count_To_One_After_Window_Passed()
        {
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.PasswordAttemptWindowSeconds).Return(60);
            var now = DateTime.Now;
            var passwordService = new PfPasswordService(settings);
            var user = new PfMembershipUser()
                {
                    FailedPasswordAttemptWindowStart = now.AddMinutes(-61),
                };

            // Act
            passwordService.UpdateFailedPasswordAttempts(user);

            // Assert
            Assert.That(user.FailedPasswordAttemptWindowStart, Is.AtLeast(now.AddSeconds(-5)));
            Assert.That(user.FailedPasswordAttemptCount, Is.EqualTo(1));
        }

        [Test]
        public void UpdateFailedPasswordAttempts_Sets_Count_To_One_If_Failure_Count_Is_Zero()
        {
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.PasswordAttemptWindowSeconds).Return(60);
            var now = DateTime.Now;
            var passwordService = new PfPasswordService(settings);
            var user = new PfMembershipUser()
            {
                FailedPasswordAttemptWindowStart = now.AddMinutes(5),
                FailedPasswordAttemptCount = 0
            };

            // Act
            passwordService.UpdateFailedPasswordAttempts(user);

            // Assert
            Assert.That(user.FailedPasswordAttemptWindowStart, Is.AtLeast(now.AddSeconds(-5)));
            Assert.That(user.FailedPasswordAttemptCount, Is.EqualTo(1));
        }

        [Test]
        public void UpdateFailedPasswordAttempts_Increments_Count_If_Failure_Count_Is_NonZero_And_MaxAttempt_Not_Breached()
        {
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.PasswordAttemptWindowSeconds).Return(60);
            settings.Expect(x => x.MaxInvalidPasswordAttempts).Return(3);
            var now = DateTime.Now;
            var passwordService = new PfPasswordService(settings);
            var user = new PfMembershipUser()
            {
                FailedPasswordAttemptWindowStart = now.AddSeconds(-30),
                FailedPasswordAttemptCount = 1
            };

            // Act
            passwordService.UpdateFailedPasswordAttempts(user);

            // Assert
            Assert.That(user.FailedPasswordAttemptCount, Is.EqualTo(2));
        }

        [Test]
        public void UpdateFailedPasswordAttempts_Locks_UserAccount_If_Failure_Count_Reaches_MaxAttempt()
        {
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.PasswordAttemptWindowSeconds).Return(60);
            settings.Expect(x => x.MaxInvalidPasswordAttempts).Return(3);
            var now = DateTime.Now;
            var passwordService = new PfPasswordService(settings);
            var user = new PfMembershipUser()
            {
                FailedPasswordAttemptWindowStart = now.AddSeconds(-30),
                FailedPasswordAttemptCount = 2
            };

            // Act
            passwordService.UpdateFailedPasswordAttempts(user);

            // Assert
            Assert.That(user.FailedPasswordAttemptCount, Is.EqualTo(3));
            Assert.That(user.IsLockedOut, Is.True);
            Assert.That(user.LastLockedOutDate, Is.AtLeast(now.AddSeconds(-5)));
        }




        [Test]
        public void UpdateFailedPasswordAnswerAttempts_Sets_Count_To_One_After_Window_Passed()
        {
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.PasswordAttemptWindowSeconds).Return(60);
            var now = DateTime.Now;
            var passwordService = new PfPasswordService(settings);
            var user = new PfMembershipUser
            {
                FailedPasswordAnswerAttemptWindowStart = now.AddMinutes(-61),
            };

            // Act
            passwordService.UpdateFailedQuestionAndAnswerAttempts(user);

            // Assert
            Assert.That(user.FailedPasswordAnswerAttemptWindowStart, Is.AtLeast(now.AddSeconds(-5)));
            Assert.That(user.FailedPasswordAnswerAttemptCount, Is.EqualTo(1));
        }

        [Test]
        public void UpdateFailedPasswordAnswerAttempts_Sets_Count_To_One_If_Failure_Count_Is_Zero()
        {
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.PasswordAttemptWindowSeconds).Return(60);
            var now = DateTime.Now;
            var passwordService = new PfPasswordService(settings);
            var user = new PfMembershipUser()
            {
                FailedPasswordAnswerAttemptWindowStart = now.AddMinutes(5),
                FailedPasswordAnswerAttemptCount = 0
            };

            // Act
            passwordService.UpdateFailedQuestionAndAnswerAttempts(user);

            // Assert
            Assert.That(user.FailedPasswordAnswerAttemptWindowStart, Is.AtLeast(now.AddSeconds(-5)));
            Assert.That(user.FailedPasswordAnswerAttemptCount, Is.EqualTo(1));
        }

        [Test]
        public void UpdateFailedPasswordAnswerAttempts_Increments_Count_If_Failure_Count_Is_NonZero_And_MaxAttempt_Not_Breached()
        {
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.PasswordAttemptWindowSeconds).Return(60);
            settings.Expect(x => x.MaxInvalidPasswordAttempts).Return(3);
            var now = DateTime.Now;
            var passwordService = new PfPasswordService(settings);
            var user = new PfMembershipUser()
            {
                FailedPasswordAnswerAttemptWindowStart = now.AddSeconds(-30),
                FailedPasswordAnswerAttemptCount = 1
            };

            // Act
            passwordService.UpdateFailedQuestionAndAnswerAttempts(user);

            // Assert
            Assert.That(user.FailedPasswordAnswerAttemptCount, Is.EqualTo(2));
        }

        [Test]
        public void UpdateFailedPasswordAnswerAttempts_Locks_UserAccount_If_Failure_Count_Reaches_MaxAttempt()
        {
            var settings = MockRepository.GenerateStub<IPfMembershipSettings>();
            settings.Expect(x => x.PasswordAttemptWindowSeconds).Return(60);
            settings.Expect(x => x.MaxInvalidPasswordAttempts).Return(3);
            var now = DateTime.Now;
            var passwordService = new PfPasswordService(settings);
            var user = new PfMembershipUser()
            {
                FailedPasswordAnswerAttemptWindowStart = now.AddSeconds(-30),
                FailedPasswordAnswerAttemptCount = 2
            };

            // Act
            passwordService.UpdateFailedQuestionAndAnswerAttempts(user);

            // Assert
            Assert.That(user.FailedPasswordAnswerAttemptCount, Is.EqualTo(3));
            Assert.That(user.IsLockedOut, Is.True);
            Assert.That(user.LastLockedOutDate, Is.AtLeast(now.AddSeconds(-5)));
        }
    }
}
