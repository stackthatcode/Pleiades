        /// <summary>
        /// TODO: separate this capability and create a standalone applet that handles this - make it part of 
        /// ... the deployment process
        /// 
        /// SOLUTION: create the UserBootstrapService
        /// 
        /// Ensures the Root User Account Exists
        /// </summary>
        // TODO: replace this with encrypted values stored in the Assembly manifest - or do the deploy thingy
        //private const string defaultCode = "#3#3#3#3";
        //private const string defaultEmail = "aleksjones@gmail.com";
        //private const string defaultQuestion = "Dad's name";
        //private const string defaultAnswer = "Donald";
 
        //public void Initialize()
        //{
        //    if (this.GetUserCountByRole(UserRole.Root) > 0)
        //    {
        //        return;
        //    }

        //    var user = this.Create(
        //        new CreateNewDomainUserRequest()
        //        {
        //            Password = defaultCode,
        //            Email = defaultEmail,
        //            PasswordQuestion = defaultQuestion,
        //            PasswordAnswer = defaultAnswer,
        //            IsApproved = true,
        //            AccountStatus = Model.AccountStatus.Active,
        //            UserRole = UserRole.Root,
        //            AccountLevel = AccountLevel.Gold,
        //            FirstName = "Aleks",
        //            LastName = "Jones"
        //        });

        //    if (user == null)
        //    {
        //        throw new Exception("Unable to create the default Root User");
        //    }
        //}