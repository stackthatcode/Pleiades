using System;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;

namespace Pleiades.Framework.Web.Security.Concrete
{
    /// <summary>
    /// Acts as intermediary between HttpContext and DomainUserService.  
    /// Also manages the cookie state via the FormsAuthenticationService.
    /// </summary>
    public class HttpContextUserService : IHttpContextUserService
    {
        public IMembershipService MembershipUserService { get; set; }
        public IDomainUserService DomainUserService { get; set; }
        public IFormsAuthenticationService FormsAuthenticationService { get; set; }


        public HttpContextUserService(IMembershipService membershipUserService,
                IDomainUserService domainUserService, IFormsAuthenticationService formsAuthenticationService)
        {
            this.MembershipUserService = membershipUserService;
            this.DomainUserService = domainUserService;
            this.FormsAuthenticationService = formsAuthenticationService;
        }

        /// <summary>
        /// Returns a Domain User based on identity stored (or not) in the HttpContext
        /// </summary>
        public DomainUser RetrieveUserFromHttpContext(HttpContextBase httpContext)
        {
            var httpContextUserName = this.GetUserNameFromContext(httpContext);

            // No username in context?  Return a blank/anon User
            if (httpContextUserName == null)
            {
                return new DomainUser();
            }

            // Attempt to retrieve the Domain User from the system
            var user = DomainUserService.RetrieveUserByMembershipUserName(httpContextUserName);

            // Does this User exist in our system?  
            if (user == null)
            {
                this.FormsAuthenticationService.ClearAuthenticationCookie();
                return new DomainUser();
            }

            // OK - we've got a valid Domain User accessing our system, so trigger a MembershipProvider "touch"
            this.MembershipUserService.Touch(user);

            // .. and return
            return user;
        }

        /// <summary>
        /// Helper method extracts the User's Identity (technically username) from the HttpContext
        /// </summary>
        public string GetUserNameFromContext(HttpContextBase context)
        {
            var identity = context.User.Identity;

            if (identity == null || identity.IsAuthenticated == false)
            {
                return null;
            }
            else
            {
                return identity.Name;
            }
        }
    }
}