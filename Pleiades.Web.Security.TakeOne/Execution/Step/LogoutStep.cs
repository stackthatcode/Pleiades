using Pleiades.Execution;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Execution.Step
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