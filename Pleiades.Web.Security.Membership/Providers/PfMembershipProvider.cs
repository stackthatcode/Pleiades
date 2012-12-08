using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using Pleiades.Data;
using Pleiades.Helpers;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Providers
{
    /// <summary>
    /// Heavily modified from original source code by Michael Ulmann, posted on CodeProject.com
    /// http://www.codeproject.com/KB/web-security/EFMembershipProvider.aspx
    /// 
    /// UPDATE: I can't recognize one bit of his original code.  Heh!
    /// </summary>
    public class PfMembershipProvider : System.Web.Security.MembershipProvider
    {
        public const int newPasswordLength = 8;

        // These are set by the Initialize method which reads from a config file
        public PfMembershipProviderSettings Settings { get; set; }


        #region Membership Provider property overrides
        public override string ApplicationName
        {
            get { return Settings.ApplicationName; }
            set { Settings.ApplicationName = value; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return Settings.EnablePasswordRetrieval; }
        }

        public override bool EnablePasswordReset
        {
            get { return Settings.EnablePasswordReset; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return Settings.RequiresQuestionAndAnswer; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return Settings.MaxInvalidPasswordAttempts; }
        }

        public override int PasswordAttemptWindow
        {
            get { return Settings.PasswordAttemptWindow; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return Settings.RequiresUniqueEmail; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return Settings.PasswordFormat; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return Settings.MinRequiredPasswordLength; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return Settings.MinRequiredNonAlphanumericCharacters; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return Settings.PasswordStrengthRegularExpression; }
        }

        public TimeSpan UserIsOnlineTimeWindow
        {
            get { return Settings.UserIsOnlineTimeWindow; }
        }

        public string Name
        {
            get { return Settings.ProviderName; }
        }
        #endregion


        #region public methods
        // Loads configuration settings which are passed from Membership
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = "Pleiades MembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Pleiades Framework MembershipProvider");
            }

            // Load the Configuration Settings into an object and set the global instance
            Settings = new PfMembershipProviderSettings(name, config);

            // Initialize the abstract base class.
            base.Initialize(name, config);
        }

        public override MembershipUser CreateUser(
                string username, string password, string email, string passwordQuestion, string passwordAnswer, 
                bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            // Validate username/password
            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            // Check whether user with passed username already exists
            var repository = PfMembershipRepositoryBroker.Create();
            var membershipUser = repository.GetUser(username);
            
            if (membershipUser != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            
            if (providerUserKey == null)
            {
                providerUserKey = Guid.NewGuid();
            }
            else
            {
                if (!(providerUserKey is Guid))
                {
                    status = MembershipCreateStatus.InvalidProviderUserKey;
                    return null;
                }
            }

            var createDate = DateTime.Now;
            var user = new Model.MembershipUser 
            {
                ProviderUserKey = (Guid)providerUserKey,
                UserName = username,
                ApplicationName = this.ApplicationName,
                Email = email,
                Password = EncodePassword(password),
                PasswordQuestion = passwordQuestion,
                PasswordAnswer = passwordAnswer,
                IsApproved = isApproved,
                LastActivityDate = createDate,
                LastLoginDate = createDate,
                LastPasswordChangedDate = createDate,
                CreationDate = createDate,
                IsOnline = false,
                IsLockedOut = false,
                LastLockedOutDate = createDate,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAttemptWindowStart = createDate,
                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = createDate,
                LastModified = createDate,
            };

            try
            {
                repository.Add(user);
                status = MembershipCreateStatus.Success;
            }
            catch(Exception ex)
            {
                // TODO - get my Logs(!!!)
                System.Diagnostics.Debug.WriteLine(ex.Message);

                status = MembershipCreateStatus.UserRejected;
            }

            return GetUser(username, false);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, 
                string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            //check if user is authenticated
            if (!ValidateUser(username, password))
            {
                return false;
            }

            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(username);
            if (user == null)
            {
                throw new ProviderException("Change password question and answer failed. No unique user found.");
            }

            user.PasswordAnswer = EncodePassword(newPasswordAnswer);
            user.PasswordQuestion = newPasswordQuestion;

            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            string password = string.Empty;

            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(username);
            if (user == null)
            {
                throw new ProviderException("Get password failed. No unique user found.");
            }
            if (user != null)
            {
                if (Convert.ToBoolean(user.IsLockedOut))
                {
                    throw new MembershipPasswordException("The supplied user is locked out.");
                }
            }
            else
            {
                throw new MembershipPasswordException("The supplied user name is not found.");
            }

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw new MembershipPasswordException("Incorrect password answer.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(user.Password);
            }

            return password;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //check if user is authenticated
            if (!ValidateUser(username, oldPassword))
            {
                return false;
            }

            // Notify that password is going to change
            var args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }
                throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
            }

            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(username);
            if (user == null)
            {
                throw new ProviderException("Change password failed. No unique user found.");
            }
            user.Password = EncodePassword(newPassword);
            user.LastPasswordChangedDate = DateTime.Now;

            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword = Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);

            var args = new ValidatePasswordEventArgs(username, newPassword, true);
            this.OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null) throw args.FailureInformation;
                throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
            }

            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(username);
            if (user == null)
            {
                throw new ProviderException("Reset password failed. No unique user found.");
            }

            if (Convert.ToBoolean(user.IsLockedOut)) 
            {
                throw new MembershipPasswordException("The supplied user is locked out.");
            }

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw new MembershipPasswordException("Incorrect password answer.");
            }

            try
            {
                user.Password = EncodePassword(newPassword);
                user.LastPasswordChangedDate = DateTime.Now;

                return newPassword;
            }
            catch
            {
                throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
            }
        }

        public override void UpdateUser(MembershipUser membershipUser)
        {
            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(membershipUser.UserName);
            if (user == null)
            {
                throw new ProviderException("Update user failed. No unique user found.");
            }

            if (this.RequiresUniqueEmail)
            {
                var userNameWithEmailAlreadyExisting = this.GetUserNameByEmail(membershipUser.Email);
                if (userNameWithEmailAlreadyExisting != String.Empty && userNameWithEmailAlreadyExisting != user.UserName)
                {
                    throw new ProviderException("Email Address already taken:" + membershipUser.Email);
                }
            }

            user.Email = membershipUser.Email;
            user.Comment = membershipUser.Comment;
            user.IsApproved = membershipUser.IsApproved;
        }

        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(username);
            if (user == null)
            {
                return false;
            }

            if (CheckPassword(password, user.Password))
            {
                if (user.IsApproved && (!user.IsLockedOut ?? false))
                {
                    isValid = true;
                    user.LastLoginDate = DateTime.Now;
                }
            }
            else
            {
                UpdateFailureCount(username, "password");
            }

            return isValid;
        }

        public override bool UnlockUser(string userName)
        {
            try
            {
                var repository = PfMembershipRepositoryBroker.Create();
                var user = repository.GetUser(userName);
                if (user == null)
                {
                    return false;
                }

                // FIXED
                user.LastLockedOutDate = DateTime.Now;
                user.IsLockedOut = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUserByProviderKey(providerUserKey);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (userIsOnline)
                {
                    user.LastActivityDate = DateTime.Now;
                }

                return user.ToSecurityMembershipUser(this.Name);
            }
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(username);
            
            if (user == null)
            {
                return null;
            }
            else
            {
                user.LastActivityDate = DateTime.Now;
                return user.ToSecurityMembershipUser(this.Name);
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            var repository = PfMembershipRepositoryBroker.Create();
            return repository.GetUserNameByEmail(email);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(username);
            if (user == null)
            {
                return false;
            }

            repository.Delete(user);

            if (deleteAllRelatedData)
            {
                // TODO: delete user related data
            }
            return true;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var users = new MembershipUserCollection();

            //retrieve all users for the current application name from the database
            var repository = PfMembershipRepositoryBroker.Create();
            var dbUsers = repository.GetAll();
            totalRecords = dbUsers.Count();
            if (totalRecords <= 0)
            {
                return users;
            }

            foreach (var user in dbUsers.Page(pageIndex, pageSize))
            {
                users.Add(user.ToSecurityMembershipUser(this.Name));
            }

            return users;
        }

        public override int GetNumberOfUsersOnline()
        {
            var repository = PfMembershipRepositoryBroker.Create();
            return repository.GetNumberOfUsersOnline(this.UserIsOnlineTimeWindow);
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var membershipUsers = new MembershipUserCollection();
            var repository = PfMembershipRepositoryBroker.Create();
            var results = repository.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
            results.ForEach(x => membershipUsers.Add(x.ToSecurityMembershipUser(this.Name)));
            return membershipUsers;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var membershipUsers = new MembershipUserCollection();
            var repository = PfMembershipRepositoryBroker.Create();
            var results = repository.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
            results.ForEach(x => membershipUsers.Add(x.ToSecurityMembershipUser(this.Name)));
            return membershipUsers;
        }
        #endregion


        #region private methods
        /// <summary>
        /// A helper method that performs the checks and updates associated with password failure tracking.
        /// </summary>
        private void UpdateFailureCount(string username, string failureType)
        {
            var repository = PfMembershipRepositoryBroker.Create();
            var user = repository.GetUser(username);

            if (user == null)
            {
                throw new ProviderException("Update failure count failed. No unique user found.");
            }

            var windowStart = new DateTime();
            int failureCount = 0;

            if (failureType == "password")
            {
                failureCount = Convert.ToInt32(user.FailedPasswordAttemptCount);
                windowStart = Convert.ToDateTime(user.FailedPasswordAttemptWindowStart);
            }

            if (failureType == "passwordAnswer")
            {
                failureCount = Convert.ToInt32(user.FailedPasswordAnswerAttemptCount);
                windowStart = Convert.ToDateTime(user.FailedPasswordAnswerAttemptWindowStart);
            }

            var windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

            if (failureCount == 0 || DateTime.Now > windowEnd)
            {
                // First password failure or outside of PasswordAttemptWindow. 
                // Start a new password failure count from 1 and a new window starting now.
                if (failureType == "password")
                {
                    user.FailedPasswordAttemptCount = 1;
                    user.FailedPasswordAttemptWindowStart = DateTime.Now;
                }
                if (failureType == "passwordAnswer")
                {
                    user.FailedPasswordAnswerAttemptCount = 1;
                    user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
                }
            }
            else
            {
                // FIXED: failure count
                failureCount++;
                if (failureCount >= MaxInvalidPasswordAttempts)
                {
                    // Max password attempts have exceeded the failure threshold. Lock out the user.
                    user.IsLockedOut = true;
                    user.LastLockedOutDate = DateTime.Now;
                    user.FailedPasswordAnswerAttemptCount = 5;
                }
                else
                {
                    // Max password attempts have not exceeded the failure threshold. Update
                    // the failure counts. Leave the window the same.
                    if (failureType == "password")
                    {
                        user.FailedPasswordAttemptCount = failureCount;
                    }
                    if (failureType == "passwordAnswer")
                    {
                        user.FailedPasswordAnswerAttemptCount = failureCount;
                    }
                }
            }
        }

        /// <summary>
        /// Compares password values based on the MembershipPasswordFormat.
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="dbpassword">database password</param>
        /// <returns>whether the passwords are identical</returns>
        protected bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            return pass1 == pass2;
        }

        /// <summary>
        /// Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        protected string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1 { Key = Settings.MachineKey.ValidationKey.HexToByte() };
                    encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        /// <summary>
        /// Decrypts or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="encodedPassword"></param>
        /// <returns></returns>
        protected string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }
        #endregion
    }
}
