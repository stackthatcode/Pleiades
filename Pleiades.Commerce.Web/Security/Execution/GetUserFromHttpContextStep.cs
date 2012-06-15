using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Interface;

namespace Pleiades.Framework.Web.Security.Execution
{
    //public class GetUserFromHttpContextStep : Step<ISecurityRequirementsContext>
    //{
    //    public GetUserFromHttpContextStep()
    //    {
    //    }

        
    //    // No username in context?  Return a blank/anon User
    //    if (httpContextUserName == null)
    //    {
    //        return new DomainUser();
    //    }

    //    // Attempt to retrieve the Domain User from the system
    //    var user = DomainUserService.RetrieveUserByMembershipUserName(httpContextUserName);

    //    // Does this User exist in our system?  
    //    if (user == null)
    //    {
    //        this.FormsAuthenticationService.ClearAuthenticationCookie();
    //        return new DomainUser();
    //    }

    //    // OK - we've got a valid Domain User accessing our system, so trigger a MembershipProvider "touch"
    //    this.MembershipUserService.Touch(user);

    //    // .. and return
    //    return user;
    //}
}
