using System;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Interface
{
    public interface IOwnerAuthorizationService
    {
        SecurityCode Authorize(int ownerUserId);
        SecurityCode Authorize(AggregateUser requestingUser, AggregateUser ownerUser);
    }
}