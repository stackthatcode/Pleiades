using System.Web.Mvc;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Rules;

namespace Pleiades.Web.Security.Interface
{
    public interface ISecurityContextFactory
    {
        SecurityContext Create(System.Web.Mvc.AuthorizationContext filterContext, AggregateUser user);
    }
}