using Pleiades.Framework.Execution;
using Pleiades.Framework.MembershipProvider.Interface;

namespace Pleiades.Framework.Web.Security.Execution.Steps
{
    public class LogoutStep : Step<BareContext>
    {
        public IFormsAuthenticationService FormsAuthService { get; set; }

        public LogoutStep(IFormsAuthenticationService formAuthService)
        {
            this.FormsAuthService = formAuthService;
        }

        public override BareContext Execute(BareContext context)
        {
            this.FormsAuthService.ClearAuthenticationCookie();
            return context;
        }
    }
}