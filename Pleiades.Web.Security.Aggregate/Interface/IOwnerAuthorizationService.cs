using System;
using Pleiades.Security;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IOwnerAuthorizationService
    {
        SecurityResponseCode Authorize(int ownerUserId);
        SecurityResponseCode Authorize(AggregateUser requestingUser, AggregateUser ownerUser);
    }
}