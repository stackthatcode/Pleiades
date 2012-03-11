using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using Pleiades.Web.Security.Data;

namespace Pleiades.Web.Security.Providers
{
    /// <summary>
    /// Entity Framework 4.0 Customer Membership Provider
    /// Source code by Michael Ulmann, posted on CodeProject.com
    /// http://www.codeproject.com/KB/web-security/EFMembershipProvider.aspx
    /// 
    /// PLEIADES BUG FIXES (discovered during integration testing):
    /// 1.) Lockout approval miscounts failed attempts - FIXED
    /// 2.) Update User doesn't push state into entity objects - FIXED
    /// 3.) Validate User allows Locked User to login - FIXED
    /// 4.) Unlock User fails to update IsLockedOut flag - FIXED
    /// </summary>
    public class MembershipProvider : System.Web.Security.MembershipProvider
    {
        #region members
        private const int newPasswordLength = 8;
        private string applicationName;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private MachineKeySection machineKey; // Used when determining encryption key values.
        private int maxInvalidPasswordAttempts;
        private int minRequiredNonAlphanumericCharacters;
        private int minRequiredPasswordLength;
        private int passwordAttemptWindow;
        private MembershipPasswordFormat passwordFormat;
        private string passwordStrengthRegularExpression;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;
        #endregion

        #region properties
        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <returns>
        /// true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.
        /// </returns>
        public override bool EnablePasswordRetrieval
        {
            get { return enablePasswordRetrieval; }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <returns>
        /// true if the membership provider supports password reset; otherwise, false. The default is true.
        /// </returns>
        public override bool EnablePasswordReset
        {
            get { return enablePasswordReset; }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <returns>
        /// true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.
        /// </returns>
        public override bool RequiresQuestionAndAnswer
        {
            get { return requiresQuestionAndAnswer; }
        }

        /// <summary>
        /// The name of the application using the custom membership provider.
        /// </summary>
        /// <returns>
        /// The name of the application using the custom membership provider.
        /// </returns>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        /// The number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </returns>
        public override int MaxInvalidPasswordAttempts
        {
            get { return maxInvalidPasswordAttempts; }
        }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        /// The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </returns>
        public override int PasswordAttemptWindow
        {
            get { return passwordAttemptWindow; }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <returns>
        /// true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.
        /// </returns>
        public override bool RequiresUniqueEmail
        {
            get { return requiresUniqueEmail; }
        }

        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.Security.MembershipPasswordFormat" /> values indicating the format for storing passwords in the data store.
        /// </returns>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return passwordFormat; }
        }

        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <returns>
        /// The minimum length required for a password. 
        /// </returns>
        public override int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
        }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>
        /// The minimum number of special characters that must be present in a valid password.
        /// </returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return minRequiredNonAlphanumericCharacters; }
        }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>
        /// A regular expression used to evaluate a password.
        /// </returns>
        public override string PasswordStrengthRegularExpression
        {
            get { return passwordStrengthRegularExpression; }
        }

        public string ConnectionString { get; set; }
        #endregion

        #region public methods
        /// <summary>
        /// Initialize this membership provider. Loads the configuration settings.
        /// </summary>
        /// <param name="name">membership provider name</param>
        /// <param name="config">configuration</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null) 
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
            {
                name = "EFMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Smart-Soft EF Membership Provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            applicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

            string temp_format = config["passwordFormat"] ?? "Hashed";

            switch (temp_format)
            {
                case "Hashed":
                    passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Initialize SqlConnection.
            ConnectionStringSettings ConnectionStringSettings = 
                    ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }
            ConnectionString = ConnectionStringSettings.ConnectionString;

            // Get encryption and decryption key information from the configuration.
            Configuration cfg;
            if (HostingEnvironment.IsHosted)
            {
                cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
                machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");
            }
            else
            {
                machineKey = (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");
            }

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear) 
                    throw new ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.");
        }

        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <returns>A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the information for the newly created user.</returns>
        /// <param name="username">The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus" /> enumeration value indicating whether the user was created successfully.</param>
        public override MembershipUser CreateUser(string username, string password, string email,
                                                  string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            // Validate username/password
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
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
            MembershipUser u = GetUser(username, false);

            if (u != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            DateTime createDate = DateTime.Now;

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

            PleiadesDB context = new PleiadesDB(ConnectionString);
            User user = new User
            {
                Id = (Guid)providerUserKey,
                Username = username,
                ApplicationName = applicationName,
                Email = email,
                Password = EncodePassword(password),
                PasswordQuestion = passwordQuestion,
                PasswordAnswer = passwordAnswer,
                IsApproved = isApproved,
                LastActivityDate = createDate,
                LastLoginDate = DateTime.Now,
                LastPasswordChangedDate = createDate,
                CreationDate = createDate,
                IsOnline = false,
                IsLockedOut = false,
                LastLockedOutDate = createDate,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAttemptWindowStart = createDate,
                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = createDate
            };

            try
            {
                context.Users.AddObject(user);
                context.SaveChanges();
                status = MembershipCreateStatus.Success;
            }
            catch
            {
                status = MembershipCreateStatus.UserRejected;
            }

            return GetUser(username, false);
        }

        /// <summary>
        /// Processes a request to update the password question and answer for a membership user.
        /// </summary>
        /// <returns>true if the password question and answer are updated successfully; otherwise, false.</returns>
        /// <param name="username">The user to change the password question and answer for.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
        /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
        public override bool ChangePasswordQuestionAndAnswer(string username, 
                string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            //check if user is authenticated
            if (!ValidateUser(username, password)) 
                return false;
            
            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Username == username && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() != 1) 
                throw new ProviderException("Change password question and answer failed. No unique user found.");

            User user = users.First();
            user.PasswordAnswer = EncodePassword(newPasswordAnswer);
            user.PasswordQuestion = newPasswordQuestion;

            try
            {
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <returns>The password for the specified user name.</returns>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval) 
                throw new ProviderException("Password Retrieval Not Enabled.");
            if (PasswordFormat == MembershipPasswordFormat.Hashed) 
                throw new ProviderException("Cannot retrieve Hashed passwords.");

            string password = string.Empty;
            PleiadesDB context = new PleiadesDB(ConnectionString);

            IQueryable<User> users = from u in context.Users
                                     where u.Username == username && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() != 1) 
                throw new ProviderException("Get password failed. No unique user found.");

            User user = users.First();
            if (user != null)
            {
                if (Convert.ToBoolean(user.IsLockedOut)) 
                    throw new MembershipPasswordException("The supplied user is locked out.");
            }
            else 
                throw new MembershipPasswordException("The supplied user name is not found.");

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw new MembershipPasswordException("Incorrect password answer.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Encrypted) 
                password = UnEncodePassword(user.Password);

            return password;
        }

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <returns>true if the password was updated successfully; otherwise, false.</returns>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //check if user is authenticated
            if (!ValidateUser(username, oldPassword)) 
                return false;

            //notify that password is going to change
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null) 
                    throw args.FailureInformation;
                throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
            }

            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Username == username && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() != 1) 
                throw new ProviderException("Change password failed. No unique user found.");

            User user = users.First();
            user.Password = EncodePassword(newPassword);
            user.LastPasswordChangedDate = DateTime.Now;

            try
            {
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <returns>The new password for the specified user.</returns>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset) 
                throw new NotSupportedException("Password reset is not enabled.");

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword = Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null) throw args.FailureInformation;
                throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
            }

            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Username == username && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() != 1) throw new ProviderException("Reset password failed. No unique user found.");

            User user = users.First();
            if (user != null)
            {
                if (Convert.ToBoolean(user.IsLockedOut)) throw new MembershipPasswordException("The supplied user is locked out.");
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

            try
            {
                user.Password = EncodePassword(newPassword);
                user.LastPasswordChangedDate = DateTime.Now;

                context.SaveChanges();
                return newPassword;
            }
            catch
            {
                throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
            }
        }

        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="membershipUser">A <see cref="T:System.Web.Security.MembershipUser" /> object that represents the user to update and the updated information for the user.</param>
        public override void UpdateUser(MembershipUser membershipUser)
        {
            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Username == membershipUser.UserName && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() != 1) 
                throw new ProviderException("Update user failed. No unique user found.");

            User user = users.First();
            if (user == null) 
                return;

            user.Email = membershipUser.Email;
            user.Comment = membershipUser.Comment;
            user.IsApproved = membershipUser.IsApproved;
            context.SaveChanges();
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <returns>true if the specified username and password are valid; otherwise, false.</returns>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Username == username && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() != 1) 
                return false;

            User user = users.First();
            if (user == null) 
                return false;

            if (CheckPassword(password, user.Password))
            {
                if (user.IsApproved && (!user.IsLockedOut ?? false))
                {
                    isValid = true;

                    user.LastLoginDate = DateTime.Now;
                    context.SaveChanges();
                }
            }
            else
            {
                UpdateFailureCount(username, "password");
            }

            return isValid;
        }

        /// <summary>
        ///  Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <returns>true if the membership user was successfully unlocked; otherwise, false.</returns>
        /// <param name="userName">The membership user whose lock status you want to clear.</param>
        public override bool UnlockUser(string userName)
        {
            try
            {
                PleiadesDB context = new PleiadesDB(ConnectionString);
                IQueryable<User> users = from u in context.Users
                                         where u.Username == userName && u.ApplicationName == applicationName
                                         select u;

                if (users.Count() != 1) return false;

                User user = users.First();
                if (user == null) return false;

                // FIXED
                user.LastLockedOutDate = DateTime.Now;
                user.IsLockedOut = false;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <returns>A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.</returns>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            MembershipUser membershipUser = null;

            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Id == (Guid)providerUserKey && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() == 1)
            {
                User user = users.First();
                if (user != null)
                {
                    membershipUser = GetMembershipUserFromPersitentObject(user);

                    if (userIsOnline)
                    {
                        user.LastActivityDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }

            return membershipUser;
        }

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <returns>A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.</returns>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser membershipUser = null;

            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Username == username && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() == 1)
            {
                User user = users.First();
                if (user != null)
                {
                    membershipUser = GetMembershipUserFromPersitentObject(user);

                    if (userIsOnline)
                    {
                        user.LastActivityDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }

            return membershipUser;
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <returns>The user name associated with the specified e-mail address. If no match is found, return null.</returns>
        /// <param name="email">The e-mail address to search for.</param>
        public override string GetUserNameByEmail(string email)
        {
            try
            {
                PleiadesDB context = new PleiadesDB(ConnectionString);
                IQueryable<User> users = from u in context.Users
                                         where u.Email == email && u.ApplicationName == applicationName
                                         select u;

                if (users.Count() != 1) 
                    return string.Empty;

                User user = users.First();
                return user != null ? user.Username : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Removes a user from the membership data source. 
        /// </summary>
        /// <returns>true if the user was successfully deleted; otherwise, false.</returns>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            try
            {
                PleiadesDB context = new PleiadesDB(ConnectionString);
                IQueryable<User> users = from u in context.Users
                                         where u.Username == username && u.ApplicationName == applicationName
                                         select u;

                if (users.Count() != 1) return false;

                User user = users.First();
                context.DeleteObject(user);
                context.SaveChanges();

                if (deleteAllRelatedData)
                {
                    // TODO: delete user related data
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection();

            //retrieve all users for the current application name from the database
            PleiadesDB context = new PleiadesDB(ConnectionString);

            totalRecords = (from u in context.Users
                            where u.ApplicationName == applicationName
                            select u).Distinct().Count();
            if (totalRecords <= 0) return users;

            IQueryable<User> efUsers = (from u in context.Users
                                        where u.ApplicationName == applicationName
                                        orderby u.Username
                                        select u).Skip(pageIndex * pageSize).Take(pageSize);

            foreach (User user in efUsers)
            {
                users.Add(GetMembershipUserFromPersitentObject(user));
            }

            return users;
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>The number of users currently accessing the application.</returns>
        public override int GetNumberOfUsersOnline()
        {
            TimeSpan onlineSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            PleiadesDB context = new PleiadesDB(ConnectionString);

            return (from u in context.Users
                    where u.ApplicationName == applicationName && u.LastActivityDate > compareTime
                    select u).Distinct().Count();
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection membershipUsers = new MembershipUserCollection();
            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Username.Contains(usernameToMatch) && u.ApplicationName == applicationName
                                     orderby u.Username
                                     select u;

            totalRecords = users.Count();
            if (users.Count() <= 0) 
                return membershipUsers;

            foreach (User user in users.Skip(pageIndex * pageSize).Take(pageSize))
            {
                membershipUsers.Add(GetMembershipUserFromPersitentObject(user));
            }

            return membershipUsers;
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection membershipUsers = new MembershipUserCollection();
            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Email.Contains(emailToMatch) && u.ApplicationName == applicationName
                                     select u;

            totalRecords = users.Count();
            if (users.Count() <= 0) 
                return membershipUsers;

            foreach (User user in users.Skip(pageIndex * pageSize).Take(pageSize))
            {
                membershipUsers.Add(GetMembershipUserFromPersitentObject(user));
            }

            return membershipUsers;
        }

        #endregion

        #region private methods
        /// <summary>
        /// A helper function that takes the current persistent user and creates a MembershiUser from the values.
        /// </summary>
        /// <param name="user">user object containing the user data retrieved from database</param>
        /// <returns>membership user object</returns>
        private MembershipUser GetMembershipUserFromPersitentObject(User user)
        {
            return new MembershipUser(Name,
                                      user.Username,
                                      user.Id,
                                      user.Email,
                                      user.PasswordQuestion,
                                      user.Comment,
                                      user.IsApproved,
                                      Convert.ToBoolean(user.IsLockedOut),
                                      Convert.ToDateTime(user.CreationDate),
                                      Convert.ToDateTime(user.LastLoginDate),
                                      Convert.ToDateTime(user.LastActivityDate),
                                      Convert.ToDateTime(user.LastPasswordChangedDate),
                                      Convert.ToDateTime(user.LastLockedOutDate));
        }

        /// <summary>
        /// A helper method that performs the checks and updates associated with password failure tracking.
        /// </summary>
        private void UpdateFailureCount(string username, string failureType)
        {
            PleiadesDB context = new PleiadesDB(ConnectionString);
            IQueryable<User> users = from u in context.Users
                                     where u.Username == username && u.ApplicationName == applicationName
                                     select u;

            if (users.Count() != 1) 
                throw new ProviderException("Update failure count failed. No unique user found.");

            User user = users.First();

            DateTime windowStart = new DateTime();
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

            DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

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

                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    throw new ProviderException("Unable to update failure count and window start.");
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

                    try
                    {
                        context.SaveChanges();
                    }
                    catch
                    {
                        throw new ProviderException("Unable to lock out user.");
                    }
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

                    try
                    {
                        context.SaveChanges();
                    }
                    catch
                    {
                        throw new ProviderException("Unable to update failure count.");
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
        private bool CheckPassword(string password, string dbpassword)
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
        private string EncodePassword(string password)
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
                    HMACSHA1 hash = new HMACSHA1 { Key = HexToByte(machineKey.ValidationKey) };
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
        private string UnEncodePassword(string encodedPassword)
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

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetConfigValue(string configValue, string defaultValue)
        {
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }
        #endregion
    }
}
